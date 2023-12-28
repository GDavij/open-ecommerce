using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Application.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Application.IntegrationEvents.Product.Events;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Domain.Contracts.Providers;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Entities.Product.ProductDetails;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Infrastructure.Providers;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.UpdateProduct;

public class UpdateProductCommandHandlerTests
{
    private readonly IStockDateTimeProvider _dateTimeProvider;
    private readonly IAppConfigService _configService;
    private readonly string _adminDashboardBaseUrl;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IStockContext _dbContext;
    private readonly IUpdateProductCommandHandler _commandHandler;

    public UpdateProductCommandHandlerTests()
    {
        _dateTimeProvider = Substitute.For<IStockDateTimeProvider>();
        _configService = Substitute.For<IAppConfigService>();
        
        _adminDashboardBaseUrl = "https://localhost:8080/dashboard";
        _configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl")
            .Returns(_adminDashboardBaseUrl);
        
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _dbContext = Substitute.For<IStockContext>();
        
        _commandHandler = new UpdateProductCommandHandler(_dbContext, _publishEndpoint, _configService, _dateTimeProvider);
    }

    [Fact]
    internal async Task ShouldUpdateProductForValidCommandWithDifferentValues()
    {
        //Arrange
        _dateTimeProvider.UtcNow
            .Returns(DateTime.UtcNow);
        
        var oldBrand = Brand.Create("brand-1-old-logo", "sells computers");
        var newBrand = Brand.Create("brand-1-new-logo", "sells computers with AI");

        DbSet<Brand> brandDbSet = new List<Brand>
            {
                oldBrand,
                newBrand
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);

        var existentProduct = Product.Create(
            brand: oldBrand,
            name: "Computer-think black version",
            description: "A Fast  computer",
            sku: "cmpt-think-b",
            ean: "a123dfg567abc",
            upc: "123def567abc",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );

        var grammeMeasureUnit = MeasureUnit.Create("Gramme", "Gram", "G");
        var megabytePerSecondMeasureUnit = MeasureUnit.Create("Megabytes Per Second", null, "MB/s");

        DbSet<MeasureUnit> measureUnitDbSet = new List<MeasureUnit>
            {
                grammeMeasureUnit,
                megabytePerSecondMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.MeasureUnits
            .Returns(measureUnitDbSet);
        
        existentProduct.Measurements = new List<MeasurementDetail>
        {
            MeasurementDetail.Create(
                product: existentProduct,
                showOrder: 1,
                name: "Weight",
                value: "590",
                measureUnit: grammeMeasureUnit),
        };

        existentProduct.TechnicalDetails = new List<TechnicalDetail>
        {
            TechnicalDetail.Create(
                product: existentProduct,
                showOrder: 1,
                name: "Connection Speed",
                value: "150",
                measureUnit: megabytePerSecondMeasureUnit)
        };

        existentProduct.OtherDetails = new List<OtherDetail>
        {
            OtherDetail.Create(
                product: existentProduct,
                showOrder: 1,
                name: "Has Ai Support",
                value: "Yes",
                measureUnit: null)
        };

        DbSet<Product> productDbSet = new List<Product>
            {
                existentProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);

        var computerTag = ProductTag.Create("Computer");
        var AiTag = ProductTag.Create("AI");

        DbSet<ProductTag> productTagDbSet = new List<ProductTag>
            {
                computerTag,
                AiTag
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.ProductTags
            .Returns(productTagDbSet);

        var command = new UpdateProductCommand
        {
            ProductId = existentProduct.Id,
            BrandId = newBrand.Id,
            Name = "Computer-think black version with AI",
            Description = "Computer-think black version with AI built-in",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "cmpt-think-b-ai",
            Price = 2999.99m,
            StockUnitCount = 100,
            TagsIds = new List<Guid>
            {
                computerTag.Id,
                AiTag.Id
            },
            Measurements = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Weight",
                    Value = "2300",
                    MeasureUnitId = grammeMeasureUnit.Id,
                    ShowOrder = 1
                }
            },
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Connection Speed",
                    Value = "500",
                    ShowOrder = 1,
                    MeasureUnitId = megabytePerSecondMeasureUnit.Id
                }
            },
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Has AI built-in",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };
        
        //Act
        var result = await _commandHandler.Handle(command, default);

        //Assert
        result.Resource
            .Should()
            .Be($"{_adminDashboardBaseUrl}/products/{existentProduct.Id}");

        await _dbContext
            .Received(1)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(1)
            .Publish(Arg.Is<ProductUpdatedIntegrationEvent>(ev =>
                ev.Product.Id == existentProduct.Id &&
                ev.Product.Brand.Id == command.BrandId &&
                ev.Product.Name == command.Name));
    }
    
     [Fact]
    internal async Task ShouldUpdateProductForValidCommandWithSameValuesAsBeforeUpdate()
    {
        //Arrange
        _dateTimeProvider.UtcNow
            .Returns(DateTime.UtcNow);
        
        var brand1 = Brand.Create("brand-1-old-logo", "sells computers");

        DbSet<Brand> brandDbSet = new List<Brand>
            {
                brand1,
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);

        var grammeMeasureUnit = MeasureUnit.Create("Gramme", "Gram", "G");
        var megabytePerSecondMeasureUnit = MeasureUnit.Create("Megabytes Per Second", null, "MB/s");
        
        DbSet<MeasureUnit> measureUnitDbSet = new List<MeasureUnit>
            {
                grammeMeasureUnit,
                megabytePerSecondMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();
        

        _dbContext.MeasureUnits
            .Returns(measureUnitDbSet);
        
        var computerTag = ProductTag.Create("Computer");
        var AiTag = ProductTag.Create("AI");

        DbSet<ProductTag> productTagDbSet = new List<ProductTag>
            {
                computerTag,
                AiTag
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.ProductTags
            .Returns(productTagDbSet);
        
        var existentProduct = Product.Create(
            brand: brand1,
            name: "Computer-think black version",
            description: "A Fast  computer",
            sku: "cmpt-think-b",
            ean: "a123dfg567abc",
            upc: "123def567abc",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );

        existentProduct.Tags = new List<ProductTag>
        {
            computerTag,
            AiTag
        };
        
        existentProduct.Measurements = new List<MeasurementDetail>
        {
            MeasurementDetail.Create(
                product: existentProduct,
                showOrder: 1,
                name: "Weight",
                value: "590",
                measureUnit: grammeMeasureUnit),
        };

        existentProduct.TechnicalDetails = new List<TechnicalDetail>
        {
            TechnicalDetail.Create(
                product: existentProduct,
                showOrder: 1,
                name: "Connection Speed",
                value: "150",
                measureUnit: megabytePerSecondMeasureUnit)
        };

        existentProduct.OtherDetails = new List<OtherDetail>
        {
            OtherDetail.Create(
                product: existentProduct,
                showOrder: 1,
                name: "Has Ai Support",
                value: "Yes",
                measureUnit: null)
        };

        DbSet<Product> productDbSet = new List<Product>
            {
                existentProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);



        var command = new UpdateProductCommand
        {
            ProductId = existentProduct.Id,
            BrandId = existentProduct.Brand.Id,
            Name = existentProduct.Name,
            Description = existentProduct.Description,
            Ean = existentProduct.EAN,
            Upc = existentProduct.UPC,
            Sku = existentProduct.SKU,
            Price = existentProduct.Price,
            StockUnitCount = existentProduct.StockUnitCount,
            TagsIds = existentProduct.Tags.Select(t => t.Id).ToList(),
            Measurements = existentProduct.Measurements.Select(m => new ProductDetailUpdateRequestPayload
            {
                Name = m.Name,
                Value = m.Value,
                ShowOrder = m.ShowOrder,
                MeasureUnitId = m.MeasureUnit?.Id
            }).ToList(),
            TechnicalDetails = existentProduct.TechnicalDetails.Select(m => new ProductDetailUpdateRequestPayload
            {
                Name = m.Name,
                Value = m.Value,
                ShowOrder = m.ShowOrder,
                MeasureUnitId = m.MeasureUnit?.Id
            }).ToList(),
            OtherDetails = existentProduct.OtherDetails.Select(m => new ProductDetailUpdateRequestPayload
            {
                Name = m.Name,
                Value = m.Value,
                ShowOrder = m.ShowOrder,
                MeasureUnitId = m.MeasureUnit?.Id
            }).ToList()
        };
        
        //Act
        var result = await _commandHandler.Handle(command, default);

        //Assert
        result.Resource
            .Should()
            .Be($"{_adminDashboardBaseUrl}/products/{existentProduct.Id}");

        await _dbContext
            .Received(1)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(1)
            .Publish(Arg.Is<ProductUpdatedIntegrationEvent>(ev =>
                ev.Product.Id == existentProduct.Id &&
                ev.Product.Brand.Id == command.BrandId &&
                ev.Product.Name == command.Name));
    }

    [Fact]
    internal async Task ShouldNotUpdateProductForInvalidCommandWithNotExistentProductId()
    {
        //Arrange
        var brand1 = Brand.Create("brand-1-old-logo", "sells computers");
        
        var existentProduct = Product.Create(
            brand: brand1,
            name: "Computer-think black version",
            description: "A Fast  computer",
            sku: "cmpt-think-b",
            ean: "a123dfg567abc",
            upc: "123def567abc",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );

        DbSet<Product> productDbSet = new List<Product>
            {
                existentProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);

        var invalidProductId = Guid.NewGuid();
        var invalidCommand = new UpdateProductCommand
        {
            ProductId = invalidProductId,
            BrandId = brand1.Id,
            Name = "Computer-think black version with AI",
            Description = "Computer-think black version with AI built-in",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "cmpt-think-b-ai",
            Price = 2999.99m,
            StockUnitCount = 100,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>(),
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>(),
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Has AI built-in",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };

        Func<Task> action = async () => { await _commandHandler.Handle(invalidCommand, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<InvalidProductException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Invalid Product was sent with Id: {invalidCommand.ProductId}");

        await _dbContext
            .Received(0)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<ProductUpdatedIntegrationEvent>());
    }

    [Fact]
    internal async Task ShouldNotUpdateProductForInvalidCommandWithNotExistentBrandId()
    {
        //Arrange
        var brand1 = Brand.Create("brand-1-old-logo", "sells computers");
        
        DbSet<Brand> brandDbSet = new List<Brand>
            {
                brand1
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);
        
        var existentProduct = Product.Create(
            brand: brand1,
            name: "Computer-think black version",
            description: "A Fast  computer",
            sku: "cmpt-think-b",
            ean: "a123dfg567abc",
            upc: "123def567abc",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );

        DbSet<Product> productDbSet = new List<Product>
            {
                existentProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);

        var invalidBrandId = Guid.NewGuid();
        var invalidCommand = new UpdateProductCommand
        {
            ProductId = existentProduct.Id,
            BrandId = invalidBrandId,
            Name = "Computer-think black version with AI",
            Description = "Computer-think black version with AI built-in",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "cmpt-think-b-ai",
            Price = 2999.99m,
            StockUnitCount = 100,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>(),
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>(),
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Has AI built-in",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };

        Func<Task> action = async () => { await _commandHandler.Handle(invalidCommand, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<InvalidBrandException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Invalid Brand was sent with id: {invalidCommand.BrandId}");

        await _dbContext
            .Received(0)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<ProductUpdatedIntegrationEvent>());
    }

    [Fact]
    internal async Task ShouldNotUpdateProductForInvalidCommandWithNameSameAsExistentProductNameThatIsNotTheOneToUpdate()
    {
        //Arrange
        var brand1 = Brand.Create("brand-1-old-logo", "sells computers");
        
        DbSet<Brand> brandDbSet = new List<Brand>
            {
                brand1
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);
        
        var existentProductToUpdate = Product.Create(
            brand: brand1,
            name: "Computer-think black version",
            description: "A Fast  computer",
            sku: "cmpt-think-b",
            ean: "a123dfg567abc",
            upc: "123def567abc",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );
        
        var existentProductExpectToShowNameError =  Product.Create(
            brand: brand1,
            name: "computer-m2",
            description: "A computer with an excellent processor...",
            sku: "cmpt-m2",
            ean: "a123dfg567kuj",
            upc: "123def567lmn",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );

        DbSet<Product> productDbSet = new List<Product>
            {
                existentProductToUpdate,
                existentProductExpectToShowNameError
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);

        var invalidCommand = new UpdateProductCommand
        {
            ProductId = existentProductToUpdate.Id,
            BrandId = brand1.Id,
            Name = existentProductExpectToShowNameError.Name,
            Description = "Computer-think black version with AI built-in",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "cmpt-think-b-ai",
            Price = 2999.99m,
            StockUnitCount = 100,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>(),
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>(),
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Has AI built-in",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };

        Func<Task> action = async () => { await _commandHandler.Handle(invalidCommand, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ExistentProductNameException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found existent product name {invalidCommand.Name}, Conflict Exception");

        await _dbContext
            .Received(0)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<ProductUpdatedIntegrationEvent>());
    }
    
    [Fact]
    internal async Task ShouldNotUpdateProductForInvalidCommandWithEanSameAsExistentProductEanThatIsNotTheOneToUpdate()
    {
        //Arrange
        var brand1 = Brand.Create("brand-1-old-logo", "sells computers");
        
        DbSet<Brand> brandDbSet = new List<Brand>
            {
                brand1
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);
        
        var existentProductToUpdate = Product.Create(
            brand: brand1,
            name: "Computer-think black version",
            description: "A Fast  computer",
            sku: "cmpt-think-b",
            ean: "a123dfg567abc",
            upc: "123def567abc",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );
        
        var existentProductExpectToShowEanError =  Product.Create(
            brand: brand1,
            name: "computer-m2",
            description: "A computer with an excellent processor...",
            sku: "cmpt-m2",
            ean: "a123dfg567kuj",
            upc: "123def567lmn",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );

        DbSet<Product> productDbSet = new List<Product>
            {
                existentProductToUpdate,
                existentProductExpectToShowEanError
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);

        var invalidCommand = new UpdateProductCommand
        {
            ProductId = existentProductToUpdate.Id,
            BrandId = brand1.Id,
            Name = "computer-m1",
            Description = "Computer-think black version with AI built-in",
            Ean = existentProductExpectToShowEanError.EAN,
            Upc = "123456789123",
            Sku = "cmpt-think-b-ai",
            Price = 2999.99m,
            StockUnitCount = 100,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>(),
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>(),
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Has AI built-in",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };

        Func<Task> action = async () => { await _commandHandler.Handle(invalidCommand, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ExistentEanCodeException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found a conflict with ean-13 {invalidCommand.Ean}, already exists!");

        await _dbContext
            .Received(0)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<ProductUpdatedIntegrationEvent>());
    }
    
    [Fact]
    internal async Task ShouldNotUpdateProductForInvalidCommandWithUpcSameAsExistentProductUpcThatIsNotTheOneToUpdate()
    {
        //Arrange
        var brand1 = Brand.Create("brand-1-old-logo", "sells computers");
        
        DbSet<Brand> brandDbSet = new List<Brand>
            {
                brand1
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);
        
        var existentProductToUpdate = Product.Create(
            brand: brand1,
            name: "Computer-think black version",
            description: "A Fast  computer",
            sku: "cmpt-think-b",
            ean: "a123dfg567abc",
            upc: "123def567abc",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );
        
        var existentProductExpectToShowUpcError =  Product.Create(
            brand: brand1,
            name: "computer-m2",
            description: "A computer with an excellent processor...",
            sku: "cmpt-m2",
            ean: "a123dfg567kuj",
            upc: "123def567lmn",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );

        DbSet<Product> productDbSet = new List<Product>
            {
                existentProductToUpdate,
                existentProductExpectToShowUpcError
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);

        var invalidCommand = new UpdateProductCommand
        {
            ProductId = existentProductToUpdate.Id,
            BrandId = brand1.Id,
            Name = "computer-m1",
            Description = "Computer-think black version with AI built-in",
            Ean = "abcdefhijklmn",
            Upc = existentProductExpectToShowUpcError.UPC,
            Sku = "cmpt-think-b-ai",
            Price = 2999.99m,
            StockUnitCount = 100,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>(),
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>(),
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Has AI built-in",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };

        Func<Task> action = async () => { await _commandHandler.Handle(invalidCommand, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ExistentUpcCodeException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found a conflict with upc {invalidCommand.Upc}, already exists!");

        await _dbContext
            .Received(0)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<ProductUpdatedIntegrationEvent>());
    }
    
    [Fact]
    internal async Task ShouldNotUpdateProductForInvalidCommandWithSkuSameAsExistentProductSkuThatIsNotTheOneToUpdate()
    {
        //Arrange
        var brand1 = Brand.Create("brand-1-old-logo", "sells computers");
        
        DbSet<Brand> brandDbSet = new List<Brand>
            {
                brand1
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);
        
        var existentProductToUpdate = Product.Create(
            brand: brand1,
            name: "Computer-think black version",
            description: "A Fast  computer",
            sku: "cmpt-think-b",
            ean: "a123dfg567abc",
            upc: "123def567abc",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );
        
        var existentProductExpectToShowSkuError =  Product.Create(
            brand: brand1,
            name: "computer-m2",
            description: "A computer with an excellent processor...",
            sku: "cmpt-m2",
            ean: "a123dfg567kuj",
            upc: "123def567lmn",
            price: 1999.99m,
            stockUnitCount: 99,
            stockDateTimeProvider: _dateTimeProvider
        );

        DbSet<Product> productDbSet = new List<Product>
            {
                existentProductToUpdate,
                existentProductExpectToShowSkuError
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Products
            .Returns(productDbSet);

        var invalidCommand = new UpdateProductCommand
        {
            ProductId = existentProductToUpdate.Id,
            BrandId = brand1.Id,
            Name = "computer-m1",
            Description = "Computer-think black version with AI built-in",
            Ean = existentProductToUpdate.EAN,
            Upc = existentProductToUpdate.UPC,
            Sku = existentProductExpectToShowSkuError.SKU,
            Price = 2999.99m,
            StockUnitCount = 100,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>(),
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>(),
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Has AI built-in",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };

        Func<Task> action = async () => { await _commandHandler.Handle(invalidCommand, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ExistentSkuCodeException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found a conflict with sku {invalidCommand.Sku}, already exists!");

        await _dbContext
            .Received(0)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<ProductUpdatedIntegrationEvent>());
    }
}