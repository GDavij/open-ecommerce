using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Core.Modules.Shared.Domain.Constants;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Product;
using Core.Modules.Stock.Application.Http.Commands.AddImageToProduct;
using Core.Modules.Stock.Domain.Constants;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;
using Core.Modules.Stock.Domain.Contracts.Providers;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Exceptions.Product;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.AddImageToProduct;

public class AddImageToProductCommandHandlerTests
{
    private readonly IAddImageToProductCommandHandler _commandHandler;
    private readonly IStockContext _dbContext;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _blobContainerClient;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;
    private readonly IStockDateTimeProvider _dateTimeProvider;

    public AddImageToProductCommandHandlerTests()
    {
        _dateTimeProvider = Substitute.For<IStockDateTimeProvider>();
        _dbContext = Substitute.For<IStockContext>();
        _configService = Substitute.For<IAppConfigService>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _blobServiceClient = Substitute.For<BlobServiceClient>();
        _blobContainerClient = Substitute.For<BlobContainerClient>();

        _commandHandler = new AddImageToProductCommandHandler(_dbContext, _blobServiceClient, _publishEndpoint, _configService);

        _blobServiceClient.GetBlobContainerClient(AzureBlobStorageContainers.ProductImages)
            .Returns(_blobContainerClient);
    }

    [Fact]
    internal async Task ShouldAddAImageIntoAlreadyExistentProduct()
    {
        //Arrange
        Brand databseBrand = Brand.Create("brand-1", "brand-1 sells construction tools");

        Product databaseProduct = Product.Create(
            brand: databseBrand,
            name: "Drill V4 880RPM",
            description: "A Fast Drill in it's V4, with good handling and design",
            sku: "br1-drv4880r",
            ean: "1234567891234",
            upc: null,
            price: 2500,
            stockUnitCount: 5,
            stockDateTimeProvider: _dateTimeProvider
        );

        DbSet<Product> mockedProductsDbContext = new List<Product>
            {
                databaseProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(mockedProductsDbContext);

        DbSet<ProductImage> mockedProductImagesDbSet = new List<ProductImage>()
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.ProductImages
            .Returns(mockedProductImagesDbSet);

        var imageFile = Substitute.For<IFormFile>();
        AddImageToProductCommand command = new AddImageToProductCommand
        {
            ProductId = databaseProduct.Id,
            Description = "Drill Photo",
            ImageFile = imageFile
        };

        imageFile.ContentType
            .Returns("application/png");

        _configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable)
            .Returns("https://localhost:3000/dashboard");

        var blobContainerMockGeneratedResourceUri = new Uri("https://opencommerce.microsoft.azure.net/blobs/container/filename.png");

        _blobContainerClient.Uri
            .Returns(blobContainerMockGeneratedResourceUri);

        string blobFileName = null;

        await _blobContainerClient.UploadBlobAsync(Arg.Do<string>(s => blobFileName = s), Arg.Any<Stream>(), default);

        //Act
        var result = await _commandHandler.Handle(command, default);

        //Assert
        await _blobContainerClient
            .Received(1)
            .UploadBlobAsync(blobFileName, Arg.Any<Stream>(), default);

        await _dbContext
            .Received(1)
            .SaveChangesAsync();

        await _publishEndpoint
            .Received(1)
            .Publish(Arg.Is<AddedImageToProductIntegrationEvent>(ev =>
                ev.ProductImage.Description == command.Description &&
                ev.ProductImage.ProductId == command.ProductId));

        string administrativeFrontendUrl = _configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable);

        result.Resource
            .Should()
            .Be($"{administrativeFrontendUrl}/products/{databaseProduct.Id}");
    }

    [Fact]
    public async Task ShouldNotAddAImageIntoANotExistentProduct()
    {
        //Arrange
        DbSet<Product> mockedProductsDbContext = new List<Product>()
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(mockedProductsDbContext);

        var imageFile = Substitute.For<IFormFile>();

        Guid notInvalidProductId = Guid.NewGuid();
        AddImageToProductCommand command = new AddImageToProductCommand
        {
            ProductId = notInvalidProductId,
            Description = "Drill Photo",
            ImageFile = imageFile
        };

        imageFile.ContentType
            .Returns("application/png");

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };

        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ProductBadRequestException>();

        //Assert
        exception.Which.Message
            .Should()
            .Be($"Could not find any product with {command.ProductId} Id, Bad Request");

        await _blobContainerClient
            .Received(0)
            .UploadBlobAsync(Arg.Any<string>(), Arg.Any<Stream>(), default);

        await _dbContext
            .Received(0)
            .SaveChangesAsync();

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Is<AddedImageToProductIntegrationEvent>(ev =>
                ev.ProductImage.Description == command.Description &&
                ev.ProductImage.ProductId == command.ProductId));
    }
}