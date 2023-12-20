using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.IntegrationEvents.StockEvents.Product.ProductCreated;
using Core.Modules.Stock.Application.Http.Commands.CreateProduct;
using Core.Modules.Stock.Application.IntegrationEvents.Product.Events.ProductCreated;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
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

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.CreateProduct;

public class CreateProductCommandHandlerTests
{
    private readonly IStockContext _context;
    private readonly IStockDateTimeProvider _stockDateTimeProvider;
    private readonly ICreateProductCommandHandler _commandHandler;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;
    public CreateProductCommandHandlerTests()
    {
        _context = Substitute.For<IStockContext>();
        _stockDateTimeProvider = Substitute.For<IStockDateTimeProvider>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _configService = Substitute.For<IAppConfigService>();
        _commandHandler = new CreateProductCommandHandler(_context, _stockDateTimeProvider, _publishEndpoint, _configService);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task ShouldCreateProductWithValidInformation(bool withSkuAndUpc)
    {
        //Arrange
        //Mock Databases 
        DbSet<Product> mockProductDbSet = new List<Product>()
            .AsQueryable()
            .BuildMockDbSet();

        var brand1 = Brand.Create("brand-1", "sells-computers");
        DbSet<Brand> mockBrandDbSet = new List<Brand>
            {
                brand1,
                Brand.Create("brand-2", "sells-computers")
            }
            .AsQueryable()
            .BuildMockDbSet();

        var tag1 = ProductTag.Create("processor-2x");
        var tag2 = ProductTag.Create("linear-algebra-processor");

        DbSet<ProductTag> mockProductTagDbSet = new List<ProductTag>
            {
                tag1,
                tag2
            }
            .AsQueryable()
            .BuildMockDbSet();


        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");
        DbSet<MeasureUnit> mockMeasureUnitDbSet = new List<MeasureUnit>
            {
                pixelMeasureUnit,
                megabytesMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _context.Products
            .ReturnsForAnyArgs(mockProductDbSet);

        _context.Brands
            .ReturnsForAnyArgs(mockBrandDbSet);

        _context.ProductTags
            .ReturnsForAnyArgs(mockProductTagDbSet);

        _context.MeasureUnits
            .ReturnsForAnyArgs(mockMeasureUnitDbSet);

        string? sku = null;
        string? upc = null;
        if (withSkuAndUpc)
        {
            sku = "brand-1-m1";
            upc = "123456789123";
        }

        _configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl")
            .Returns("https://localhost:3000/dashboard");
        
        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Upc = upc,
            Sku = sku,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = brand1.Id,
            TagsIds = new List<Guid>
            {
                tag1.Id,
                tag2.Id
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Width",
                    Value = "1920",
                    ShowOrder = 1,
                    MeasureUnitId = pixelMeasureUnit.Id
                },
                new ProductDetailRequestPayload
                {
                    Name = "Height",
                    Value = "1080",
                    ShowOrder = 2,
                    MeasureUnitId = megabytesMeasureUnit.Id
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Resolution",
                    Value = "8K",
                    ShowOrder = 1,
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Has Support to brand-1 Ai TPU runner system",
                    Value = "False",
                    ShowOrder = 1
                }
            },
        };

        Product createdProduct = null!;
        _context.Products.Add(Arg.Do<Product>(p => createdProduct = p));

        _publishEndpoint.Publish(Arg.Any<ProductCreatedIntegrationEvent>())
            .Returns(Task.CompletedTask);
        
        //Act
        var result = await _commandHandler.Handle(command, default);

        //Assert
        _context.Products
            .Received(1)
            .Add(Arg.Any<Product>());
        
        await _context
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        await _publishEndpoint
            .Received(1)
            .Publish(Arg.Is<ProductCreatedIntegrationEvent>(ev => ev.Product.Id == createdProduct.MapToProductDto().Id));

        result
            .Should()
            .NotBe(null);
        
        string administrativeFrontendUrl = _configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl");
        result.Resource
            .Should()
            .Be($"{administrativeFrontendUrl}/products/{createdProduct.Id}");
        
        createdProduct.Name
            .Should()
            .Be(command.Name);

        createdProduct.Description
            .Should()
            .Be(command.Description);

        createdProduct.Brand.Id
            .Should()
            .Be(command.BrandId);
        
        createdProduct.CreatedAt
            .Should()
            .Be(_stockDateTimeProvider.UtcNow);
    }

    [Fact]
    internal async Task ShouldNotCreateProductWithNotExistentBrand()
    {
        //Arrange
        //Mock Databases 
        DbSet<Product> mockProductDbSet = new List<Product>()
            .AsQueryable()
            .BuildMockDbSet();

        var brand1 = Brand.Create("brand-1", "sells-computers");
        var brand2 = Brand.Create("brand-2", "sells-computers");
        DbSet<Brand> mockBrandDbSet = new List<Brand>
            {
                brand1,
                brand2
            }
            .AsQueryable()
            .BuildMockDbSet();

        var tag1 = ProductTag.Create("processor-2x");
        var tag2 = ProductTag.Create("linear-algebra-processor");
        DbSet<ProductTag> mockProductTagDbSet = new List<ProductTag>
            {
                tag1,
                tag2
            }
            .AsQueryable()
            .BuildMockDbSet();

        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");
        DbSet<MeasureUnit> mockMeasureUnitDbSet = new List<MeasureUnit>
            {
                pixelMeasureUnit,
                megabytesMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _context.Products
            .ReturnsForAnyArgs(mockProductDbSet);

        _context.Brands
            .ReturnsForAnyArgs(mockBrandDbSet);

        _context.ProductTags
            .ReturnsForAnyArgs(mockProductTagDbSet);

        _context.MeasureUnits
            .ReturnsForAnyArgs(mockMeasureUnitDbSet);

        Guid invalidBrandId = Guid.NewGuid();
        
        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Upc = null,
            Sku = null,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = invalidBrandId,
            TagsIds = new List<Guid>
            {
                tag1.Id,
                tag2.Id
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Width",
                    Value = "1920",
                    ShowOrder = 1,
                    MeasureUnitId = pixelMeasureUnit.Id
                },
                new ProductDetailRequestPayload
                {
                    Name = "Height",
                    Value = "1080",
                    ShowOrder = 2,
                    MeasureUnitId = megabytesMeasureUnit.Id
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Resolution",
                    Value = "8K",
                    ShowOrder = 1,
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Has Support to brand-1 Ai TPU runner system",
                    Value = "False",
                    ShowOrder = 1
                }
            },
        };
        
        Func<Task> action = async () => await _commandHandler.Handle(command, default);
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<InvalidBrandException>("Brand does not exists in the system");
        
        //Act
        exception.Which.Message
            .Should()
            .Be($"Invalid Brand was sent with id: {invalidBrandId}");
        
        await _context.Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    internal async Task ShouldNotCreateProductWithSameEanOfAExistentProduct()
    {
        //Arrange
        //Mock Databases 
        var existentEan13 = "1234567891234";
        
        var brand1 = Brand.Create("brand-1", "sells-computers");
        Product existentProduct = Product.Create(
            brand: brand1,
            name: "computer-m3-arm",
            description: null,
            ean: existentEan13,
            sku: null,
            upc: null,
            price: 99999,
            stockUnitCount: 5,
            stockDateTimeProvider: _stockDateTimeProvider
        );
        
        DbSet<Product> mockProductDbSet = new List<Product>
            {
                existentProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        var brand2 = Brand.Create("brand-2", "sells-computers");
        DbSet<Brand> mockBrandDbSet = new List<Brand>
            {
                brand1,
                brand2
            }
            .AsQueryable()
            .BuildMockDbSet();

        var tag1 = ProductTag.Create("processor-2x");
        var tag2 = ProductTag.Create("linear-algebra-processor");
        DbSet<ProductTag> mockProductTagDbSet = new List<ProductTag>
            {
                tag1,
                tag2
            }
            .AsQueryable()
            .BuildMockDbSet();

        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");
        DbSet<MeasureUnit> mockMeasureUnitDbSet = new List<MeasureUnit>
            {
                pixelMeasureUnit,
                megabytesMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _context.Products
            .ReturnsForAnyArgs(mockProductDbSet);

        _context.Brands
            .ReturnsForAnyArgs(mockBrandDbSet);

        _context.ProductTags
            .ReturnsForAnyArgs(mockProductTagDbSet);

        _context.MeasureUnits
            .ReturnsForAnyArgs(mockMeasureUnitDbSet);
        
        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = existentEan13,
            Upc = null,
            Sku = null,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = brand1.Id,
            TagsIds = new List<Guid>
            {
                tag1.Id,
                tag2.Id
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Width",
                    Value = "1920",
                    ShowOrder = 1,
                    MeasureUnitId = pixelMeasureUnit.Id
                },
                new ProductDetailRequestPayload
                {
                    Name = "Height",
                    Value = "1080",
                    ShowOrder = 2,
                    MeasureUnitId = megabytesMeasureUnit.Id
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Resolution",
                    Value = "8K",
                    ShowOrder = 1,
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Has Support to brand-1 Ai TPU runner system",
                    Value = "False",
                    ShowOrder = 1
                }
            },
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ExistentEanCodeException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found a conflict with ean-13 {existentEan13}, already exists!");

        await _context.Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    internal async Task ShouldNotCreateProductWithSameUpcOfExistentProduct()
    {
        //Arrange
        //Mock Databases
        var existentUpca = "123456789123";
        
        var brand1 = Brand.Create("brand-1", "sells-computers");
        Product existentProduct = Product.Create(
            brand: brand1,
            name: "computer-m3-arm",
            description: null,
            ean: "abcdefghijkln",
            sku: null,
            upc: existentUpca,
            price: 99999,
            stockUnitCount: 5,
            stockDateTimeProvider: _stockDateTimeProvider
        );
        
        DbSet<Product> mockProductDbSet = new List<Product>
            {
                existentProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        var brand2 = Brand.Create("brand-2", "sells-computers");
        DbSet<Brand> mockBrandDbSet = new List<Brand>
            {
                brand1,
                brand2
            }
            .AsQueryable()
            .BuildMockDbSet();

        var tag1 = ProductTag.Create("processor-2x");
        var tag2 = ProductTag.Create("linear-algebra-processor");
        DbSet<ProductTag> mockProductTagDbSet = new List<ProductTag>
            {
                tag1,
                tag2
            }
            .AsQueryable()
            .BuildMockDbSet();

        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");
        DbSet<MeasureUnit> mockMeasureUnitDbSet = new List<MeasureUnit>
            {
                pixelMeasureUnit,
                megabytesMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _context.Products
            .ReturnsForAnyArgs(mockProductDbSet);

        _context.Brands
            .ReturnsForAnyArgs(mockBrandDbSet);

        _context.ProductTags
            .ReturnsForAnyArgs(mockProductTagDbSet);

        _context.MeasureUnits
            .ReturnsForAnyArgs(mockMeasureUnitDbSet);
        
        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Upc = existentUpca,
            Sku = null,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = brand1.Id,
            TagsIds = new List<Guid>
            {
                tag1.Id,
                tag2.Id
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Width",
                    Value = "1920",
                    ShowOrder = 1,
                    MeasureUnitId = pixelMeasureUnit.Id
                },
                new ProductDetailRequestPayload
                {
                    Name = "Height",
                    Value = "1080",
                    ShowOrder = 2,
                    MeasureUnitId = megabytesMeasureUnit.Id
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Resolution",
                    Value = "8K",
                    ShowOrder = 1,
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Has Support to brand-1 Ai TPU runner system",
                    Value = "False",
                    ShowOrder = 1
                }
            },
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ExistentUpcCodeException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found a conflict with upc {existentUpca}, already exists!");

        await _context.Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    internal async Task ShouldNotCreateProductWithSameSkuOfExistentProduct()
    {
        //Arrange
        //Mock Databases
        var existentSku = "brand-1-computer";
        
        var brand1 = Brand.Create("brand-1", "sells-computers");
        Product existentProduct = Product.Create(
            brand: brand1,
            name: "computer-m3-arm",
            description: null,
            ean: "abcdefghijkln",
            sku: existentSku,
            upc: null,
            price: 99999,
            stockUnitCount: 5,
            stockDateTimeProvider: _stockDateTimeProvider
        );
        
        DbSet<Product> mockProductDbSet = new List<Product>
            {
                existentProduct
            }
            .AsQueryable()
            .BuildMockDbSet();

        var brand2 = Brand.Create("brand-2", "sells-computers");
        DbSet<Brand> mockBrandDbSet = new List<Brand>
            {
                brand1,
                brand2
            }
            .AsQueryable()
            .BuildMockDbSet();

        var tag1 = ProductTag.Create("processor-2x");
        var tag2 = ProductTag.Create("linear-algebra-processor");
        DbSet<ProductTag> mockProductTagDbSet = new List<ProductTag>
            {
                tag1,
                tag2
            }
            .AsQueryable()
            .BuildMockDbSet();

        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");
        DbSet<MeasureUnit> mockMeasureUnitDbSet = new List<MeasureUnit>
            {
                pixelMeasureUnit,
                megabytesMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _context.Products
            .ReturnsForAnyArgs(mockProductDbSet);

        _context.Brands
            .ReturnsForAnyArgs(mockBrandDbSet);

        _context.ProductTags
            .ReturnsForAnyArgs(mockProductTagDbSet);

        _context.MeasureUnits
            .ReturnsForAnyArgs(mockMeasureUnitDbSet);
        
        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Upc = null,
            Sku = existentSku,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = brand1.Id,
            TagsIds = new List<Guid>
            {
                tag1.Id,
                tag2.Id
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Width",
                    Value = "1920",
                    ShowOrder = 1,
                    MeasureUnitId = pixelMeasureUnit.Id
                },
                new ProductDetailRequestPayload
                {
                    Name = "Height",
                    Value = "1080",
                    ShowOrder = 2,
                    MeasureUnitId = megabytesMeasureUnit.Id
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Resolution",
                    Value = "8K",
                    ShowOrder = 1,
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Has Support to brand-1 Ai TPU runner system",
                    Value = "False",
                    ShowOrder = 1
                }
            },
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ExistentSkuCodeException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found a conflict with sku {existentSku}, already exists!");

        await _context.Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    internal async Task ShouldNotCreateProductWithAnyTagThatDoesNotExistInTheSystem()
    {
        //Arrange
        //Mock Databases 
        DbSet<Product> mockProductDbSet = new List<Product>()
            .AsQueryable()
            .BuildMockDbSet();

        var brand1 = Brand.Create("brand-1", "sells-computers");
        DbSet<Brand> mockBrandDbSet = new List<Brand>
            {
                brand1,
                Brand.Create("brand-2", "sells-computers")
            }
            .AsQueryable()
            .BuildMockDbSet();

        var tag1 = ProductTag.Create("processor-2x");
        var tag2 = ProductTag.Create("linear-algebra-processor");

        DbSet<ProductTag> mockProductTagDbSet = new List<ProductTag>
            {
                tag1,
                tag2
            }
            .AsQueryable()
            .BuildMockDbSet();


        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");
        DbSet<MeasureUnit> mockMeasureUnitDbSet = new List<MeasureUnit>
            {
                pixelMeasureUnit,
                megabytesMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _context.Products
            .ReturnsForAnyArgs(mockProductDbSet);

        _context.Brands
            .ReturnsForAnyArgs(mockBrandDbSet);

        _context.ProductTags
            .ReturnsForAnyArgs(mockProductTagDbSet);

        _context.MeasureUnits
            .ReturnsForAnyArgs(mockMeasureUnitDbSet);

        Guid invalidTagId = Guid.NewGuid();
        
        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Upc = null,
            Sku = null,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = brand1.Id,
            TagsIds = new List<Guid>
            {
                tag1.Id,
                tag2.Id,
                invalidTagId
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Width",
                    Value = "1920",
                    ShowOrder = 1,
                    MeasureUnitId = pixelMeasureUnit.Id
                },
                new ProductDetailRequestPayload
                {
                    Name = "Height",
                    Value = "1080",
                    ShowOrder = 2,
                    MeasureUnitId = megabytesMeasureUnit.Id
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Resolution",
                    Value = "8K",
                    ShowOrder = 1,
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Has Support to brand-1 Ai TPU runner system",
                    Value = "False",
                    ShowOrder = 1
                }
            },
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };
        
        //Act 
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<InvalidProductTagException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Invalid Tag was sent, id is {invalidTagId}");
    }

    [Theory]
    [InlineData(InvalidProductDetailToTest.Measures)]
    [InlineData(InvalidProductDetailToTest.Technicals)]
    [InlineData(InvalidProductDetailToTest.Others)]
    internal async Task ShouldNotCreateProductWithAnyProductDetailsThatHasANotExistentMeasure(InvalidProductDetailToTest productDetailToTestValidation)
    {
        //Arrange
        //Mock Databases 
        DbSet<Product> mockProductDbSet = new List<Product>()
            .AsQueryable()
            .BuildMockDbSet();

        var brand1 = Brand.Create("brand-1", "sells-computers");
        DbSet<Brand> mockBrandDbSet = new List<Brand>
            {
                brand1,
                Brand.Create("brand-2", "sells-computers")
            }
            .AsQueryable()
            .BuildMockDbSet();

        var tag1 = ProductTag.Create("processor-2x");
        var tag2 = ProductTag.Create("linear-algebra-processor");

        DbSet<ProductTag> mockProductTagDbSet = new List<ProductTag>
            {
                tag1,
                tag2
            }
            .AsQueryable()
            .BuildMockDbSet();


        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");
        DbSet<MeasureUnit> mockMeasureUnitDbSet = new List<MeasureUnit>
            {
                pixelMeasureUnit,
                megabytesMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _context.Products
            .ReturnsForAnyArgs(mockProductDbSet);

        _context.Brands
            .ReturnsForAnyArgs(mockBrandDbSet);

        _context.ProductTags
            .ReturnsForAnyArgs(mockProductTagDbSet);

        _context.MeasureUnits
            .ReturnsForAnyArgs(mockMeasureUnitDbSet);

        var measureDetails = new List<ProductDetailRequestPayload>
        {
            new ProductDetailRequestPayload
            {
                Name = "Some Measure Detail",
                Value = "Value",
                ShowOrder = 1,
                MeasureUnitId = pixelMeasureUnit.Id
            },
            new ProductDetailRequestPayload
            {
                Name = "Some Measure Details",
                Value = "Value",
                ShowOrder = 2,
                MeasureUnitId = megabytesMeasureUnit.Id
            }
        };

        var technicalDetails = new List<ProductDetailRequestPayload>
        {
            new ProductDetailRequestPayload
            {
                Name = "Technical Detail",
                Value = "A Value",
                ShowOrder = 1,
            }
        };

        var otherDetails = new List<ProductDetailRequestPayload>
        {
            new ProductDetailRequestPayload
            {
                Name = "Some Other Details",
                Value = "A Value",
                ShowOrder = 1
            }
        };

        Guid invalidMeasureId = Guid.NewGuid();
        switch (productDetailToTestValidation)
        {
            case InvalidProductDetailToTest.Measures:
               measureDetails.Add(new ProductDetailRequestPayload
               {
                   Name = "Some Invalid Measure Detail Measure Unit",
                   Value = "Some Invalid Value",
                   ShowOrder = 3,
                   MeasureUnitId = invalidMeasureId
               });
               break;
            
            case InvalidProductDetailToTest.Technicals:
                technicalDetails.Add(new ProductDetailRequestPayload
                {
                    Name = "Some Invalid Technical Detail Measure Unit",
                    Value = "Some Invalid Value",
                    ShowOrder = 2,
                    MeasureUnitId = invalidMeasureId
                });
                break;
            
            case InvalidProductDetailToTest.Others:
                otherDetails.Add(new ProductDetailRequestPayload
                {
                    Name = "Some Invalid Other Detail Measure Unit",
                    Value = "Some Invalid Value",
                    ShowOrder = 2,
                    MeasureUnitId = invalidMeasureId
                });
                break;
        }
        
        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Upc = null,
            Sku = null,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = brand1.Id,
            TagsIds = new List<Guid>
            {
                tag1.Id,
                tag2.Id,
            },
            Measurements = measureDetails,
            TechinicalDetails = technicalDetails,
            OtherDetails = otherDetails
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };
        
        //Act 
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<InvalidMeasureUnitException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Invalid Measure Unit was sent with Id: {invalidMeasureId}");
    }

    [Theory]
    [InlineData(InvalidProductDetailToTest.Measures)]
    [InlineData(InvalidProductDetailToTest.Technicals)]
    [InlineData(InvalidProductDetailToTest.Others)]
    internal async Task ShouldNotCreateProductWithAnyProductDetailsWithRepeatedShowOrderInProductDetailLists(InvalidProductDetailToTest invalidProductDetailToTest)
    {
         //Mock Databases 
        DbSet<Product> mockProductDbSet = new List<Product>()
            .AsQueryable()
            .BuildMockDbSet();
         
        var brand1 = Brand.Create("brand-1", "sells-computers");
        DbSet<Brand> mockBrandDbSet = new List<Brand>
            {
                brand1,
                Brand.Create("brand-2", "sells-computers")
            }
            .AsQueryable()
            .BuildMockDbSet();

        var tag1 = ProductTag.Create("processor-2x");
        var tag2 = ProductTag.Create("linear-algebra-processor");

        DbSet<ProductTag> mockProductTagDbSet = new List<ProductTag>
            {
                tag1,
                tag2
            }
            .AsQueryable()
            .BuildMockDbSet();


        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");
        DbSet<MeasureUnit> mockMeasureUnitDbSet = new List<MeasureUnit>
            {
                pixelMeasureUnit,
                megabytesMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _context.Products
            .ReturnsForAnyArgs(mockProductDbSet);

        _context.Brands
            .ReturnsForAnyArgs(mockBrandDbSet);

        _context.ProductTags
            .ReturnsForAnyArgs(mockProductTagDbSet);

        _context.MeasureUnits
            .ReturnsForAnyArgs(mockMeasureUnitDbSet);
        

        _configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl")
            .Returns("https://localhost:3000/dashboard");


        var measurements = new List<ProductDetailRequestPayload>();
        var technicalDetails = new List<ProductDetailRequestPayload>();
        var otherDetails = new List<ProductDetailRequestPayload>();

        switch (invalidProductDetailToTest)
        {
            case InvalidProductDetailToTest.Measures:
                measurements.AddRange(new List<ProductDetailRequestPayload>
                {
                    new ProductDetailRequestPayload
                    {
                        Name = "Width",
                        Value = "1920",
                        ShowOrder = 1,
                        MeasureUnitId = pixelMeasureUnit.Id
                    },
                    new ProductDetailRequestPayload
                    {
                        Name = "Height",
                        Value = "1080",
                        ShowOrder = 1,
                        MeasureUnitId = pixelMeasureUnit.Id
                    }
                });
                break;
            
            case InvalidProductDetailToTest.Technicals:
                technicalDetails.AddRange(new List<ProductDetailRequestPayload>
                {
                    new ProductDetailRequestPayload
                    {
                        Name = "DDR5 Support",
                        Value = "Yes",
                        MeasureUnitId = null,
                        ShowOrder = 1
                    },
                    new ProductDetailRequestPayload
                    {
                        Name = "Architecture",
                        Value = "ARM64",
                        ShowOrder = 1,
                        MeasureUnitId = null
                    }
                });
                break;
            
            case InvalidProductDetailToTest.Others:
                otherDetails.AddRange(new List<ProductDetailRequestPayload>
                {
                    new ProductDetailRequestPayload
                    {
                        Name = "Has brand-1 AI services",
                        Value = "Yes",
                        ShowOrder = 1,
                        MeasureUnitId = null
                    },
                    new ProductDetailRequestPayload
                    {
                        Name = "Has brand-1 realtime X86 translate support to ARM",
                        Value = "Yes",
                        ShowOrder = 1,
                        MeasureUnitId = null
                    }
                });
                break;
        }
        
        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = brand1.Id,
            TagsIds = new List<Guid>
            {
                tag1.Id,
                tag2.Id
            },
            Measurements = measurements,
            TechinicalDetails = technicalDetails,
            OtherDetails = otherDetails
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };
        
        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<ShowOrderRepeatedException>();
        
        //Assert 
        switch (invalidProductDetailToTest)
        {
            case InvalidProductDetailToTest.Measures:
                exception.Which.Message
                    .Should()
                    .Be($"Encountered repeated value at list {ShowOrderRepeatedEncountered.Measures}");
                break;
            
            case InvalidProductDetailToTest.Technicals:
                exception.Which.Message
                    .Should()
                    .Be($"Encountered repeated value at list {ShowOrderRepeatedEncountered.TechnicalDetails}");
                break;
            
            case InvalidProductDetailToTest.Others:
                exception.Which.Message
                    .Should()
                    .Be($"Encountered repeated value at list {ShowOrderRepeatedEncountered.OtherDetails}");
                break;
        }
    }
    
    internal enum InvalidProductDetailToTest
    {
        Measures,
        Technicals,
        Others
    }
}