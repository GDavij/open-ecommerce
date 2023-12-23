using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Modules.Stock.Application.Http.Commands.RemoveImageFromProduct;
using Core.Modules.Stock.Application.IntegrationEvents.Product.Events.RemoveImageFromProductIntegrationEvent;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;
using Core.Modules.Stock.Domain.Contracts.Providers;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Exceptions.Product;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.RemoveImageFromProduct;

public class RemoveImageFromProductCommandHandlerTests
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IStockDateTimeProvider _dateTimeProvider;
    
    private readonly IRemoveImageFromProductCommandHandler _commandHandler;
    
    public RemoveImageFromProductCommandHandlerTests()
    {
        _dbContext = Substitute.For<IStockContext>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _dateTimeProvider = Substitute.For<IStockDateTimeProvider>();
        
        _commandHandler = new RemoveImageFromProductCommandHandler(_dbContext, _publishEndpoint);
    }
    
    [Fact]
    internal async Task ShouldRemoveImageReferenceAndNotifyOtherModulesSuccessfully()
    {
        //Arrange
        _dateTimeProvider.UtcNow
            .Returns(DateTime.UtcNow);
        
        var existentBrand = Brand.Create("brand-1", "sells controllers for consoles");

        var existentProduct = Product.Create(
            brand: existentBrand,
            name: "PS4 Controller",
            description: "A really good controller with many buttons",
            sku: null,
            ean: "1234567891234",
            upc: null,
            price: 512,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider);
        
        DbSet<Product> productDbSet = new List<Product>
            {
                existentProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);
        
       var existentImage = ProductImage.Create(existentProduct, "PS4 controller version 2022", "https://localhost.mock.test/url:8080");
        DbSet<ProductImage> productImagesDbSet = new List<ProductImage>
            {
                existentImage
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.ProductImages
            .Returns(productImagesDbSet);

        var command = new RemoveImageFromProductCommand
        {
            Id = existentImage.Id
        };
        
        //Act
        var result = await _commandHandler.Handle(command, default);
        
        //Assert
        result.Success
            .Should()
            .BeTrue();
        
        await _dbContext
            .Received(1)
            .SaveChangesAsync();
        
        await _publishEndpoint
            .Received(1)
            .Publish(Arg.Is<RemoveImageFromProductIntegrationEvent>(ev => ev.Id == existentImage.Id && ev.ProductId == existentProduct.Id));
    }

    [Fact]
    public async Task ShouldNotRemoveImageWhenImageNotExistent()
    {
        //Arrange
        _dateTimeProvider.UtcNow
            .Returns(DateTime.UtcNow);
        
        var existentBrand = Brand.Create("brand-1", "sells controllers for consoles");

        var existentProduct = Product.Create(
            brand: existentBrand,
            name: "PS4 Controller",
            description: "A really good controller with many buttons",
            sku: null,
            ean: "1234567891234",
            upc: null,
            price: 512,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider);
        
        DbSet<ProductImage> productImagesDbSet = new List<ProductImage>()
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.ProductImages
            .Returns(productImagesDbSet);

        Guid invalidProductImageId = Guid.NewGuid();
        
        var command = new RemoveImageFromProductCommand
        {
            Id = invalidProductImageId
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command,default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ProductImageBadRequestException>();

        //Assert
        exception.Which.Message
            .Should()
            .Be($"Could not find any product image with {invalidProductImageId} Id, Bad Request");

        await _dbContext
            .Received(0)
            .SaveChangesAsync();

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<RemoveImageFromProductIntegrationEvent>());
    }
}