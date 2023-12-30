using System;
using System.Collections.Generic;
using Core.Modules.Stock.Application.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.UpdateProduct;

public class UpdateProductCommandValidatorTests
{
    private readonly AbstractValidator<UpdateProductCommand> _validator;

    public UpdateProductCommandValidatorTests()
    {
        _validator = new UpdateProductCommandValidator();
    }

    [Fact]
    internal void ShouldAcceptValidCommand()
    {
        //Arrange
        var command = new UpdateProductCommand
        {
            ProductId = Guid.NewGuid(),
            BrandId = Guid.NewGuid(),
            Name = "nouveau GPU",
            Description = "Runs great on linux",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "nv-gpu",
            Price = 100.00m,
            StockUnitCount = 9,
            TagsIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            },
            Measurements = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Weight",
                    Value = "100",
                    ShowOrder = 1,
                    MeasureUnitId = Guid.NewGuid()
                }
            },
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Support Wayland",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                },
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Support Xorg",
                    Value = "Yes",
                    ShowOrder = 2,
                    MeasureUnitId = null
                }
            },
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = "Has Manual",
                    Value = "Yes",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };
        
        //Act
        var results = _validator.Validate(command);
        
        //Assert
        results.IsValid
            .Should()
            .BeTrue();

        results.Errors.Count
            .Should()
            .Be(0);
    }
    
    [Fact]
    internal void ShouldAcceptValidCommandWithEmptyListsForProductDetailsAndTags()
    {
        //Arrange
        var command = new UpdateProductCommand
        {
            ProductId = Guid.NewGuid(),
            BrandId = Guid.NewGuid(),
            Name = "nouveau GPU",
            Description = "Runs great on linux",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "nv-gpu",
            Price = 100.00m,
            StockUnitCount = 9,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>(),
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>(),
            OtherDetails = new List<ProductDetailUpdateRequestPayload>()
        };
        
        //Act
        var results = _validator.Validate(command);
        
        //Assert
        results.IsValid
            .Should()
            .BeTrue();

        results.Errors.Count
            .Should()
            .Be(0);
    }
    
    [Fact]
    internal void ShouldNegateInvalidCommandWithEmptyValues()
    {
        //Arrange
        var command = new UpdateProductCommand
        {
            ProductId = default,
            BrandId = default,
            Name = default,
            Description = default,
            Ean = default,
            Upc = default,
            Sku = default,
            Price = default,
            StockUnitCount = default,
            TagsIds = new List<Guid>
            {
                Guid.NewGuid(),
                default
            },
            Measurements = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = default,
                    Value = default,
                    ShowOrder = default,
                    MeasureUnitId = default
                }
            },
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = default,
                    Value = default,
                    ShowOrder = default,
                    MeasureUnitId = default
                },
                new ProductDetailUpdateRequestPayload
                {
                    Name = default,
                    Value = default,
                    ShowOrder = default,
                    MeasureUnitId = default
                }
            },
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = default,
                    Value = default,
                    ShowOrder = default,
                    MeasureUnitId = default
                }
            }
        };
        
        //Act
        var results = _validator.Validate(command);
        
        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(23);
    }

    [Fact]
    internal void ShouldNegateInvalidCommandWithValuesLessThanMinValues()
    {
        //Arrange
        var command = new UpdateProductCommand
        {
            ProductId = Guid.NewGuid(),
            BrandId = Guid.NewGuid(),
            Name = "Nouveau GPU",
            Description = "Open Source GPU",
            Ean = "123456789123",
            Upc = "12345678912",
            Sku = "nvg",
            Price = -100.99m,
            StockUnitCount = -10,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>(),
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>(),
            OtherDetails = new List<ProductDetailUpdateRequestPayload>()
        };
        
        //Act
        var results = _validator.Validate(command);
        
        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(5);
    }
    
    [Fact]
    internal void ShouldNegateInvalidCommandWithValuesHigherThanMaxValues()
    {
        //Arrange
        var command = new UpdateProductCommand
        {
            ProductId = Guid.NewGuid(),
            BrandId = Guid.NewGuid(),
            Name = new string('*', 256),
            Description = new string('*', 513),
            Ean = new string('*', 14),
            Upc = new string('*', 13),
            Sku =  new string('*', 21),
            Price = 999.99m,
            StockUnitCount = 200,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = new string('*', 256),
                    Value = new string('*', 256),
                    ShowOrder = 30,
                    MeasureUnitId = null
                }
            },
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = new string('-', 256),
                    Value = new string(')', 256),
                    ShowOrder = 10,
                    MeasureUnitId = Guid.NewGuid()
                },
                new ProductDetailUpdateRequestPayload
                {
                    Name = new string('@', 296),
                    Value = new string('$', 256),
                    ShowOrder = 123,
                    MeasureUnitId = null
                }
            },
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = new string('%', 500),
                    Value = new string('^', 257),
                    ShowOrder = 2,
                    MeasureUnitId = Guid.NewGuid()
                }
            }
        };
        
        //Act
        var results = _validator.Validate(command);
        
        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(13);
    }

    [Fact]
    internal void ShouldNegateInvalidCommandWithNullValuesConsideringListValuesAsNull()
    {
            //Arrange
        var command = new UpdateProductCommand
        {
            ProductId = Guid.NewGuid(),
            BrandId = Guid.NewGuid(),
            Name = "nouveau GPU",
            Description = "Runs great on linux",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "nv-gpu",
            Price = 100.00m,
            StockUnitCount = 9,
            TagsIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            },
            Measurements = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = null,
                    Value = null,
                    ShowOrder = 1,
                    MeasureUnitId = Guid.NewGuid()
                }
            },
            TechnicalDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = null,
                    Value = null,
                    ShowOrder = 1,
                    MeasureUnitId = null
                },
                new ProductDetailUpdateRequestPayload
                {
                    Name = null,
                    Value = null,
                    ShowOrder = 1,
                    MeasureUnitId = Guid.NewGuid()
                }
            },
            OtherDetails = new List<ProductDetailUpdateRequestPayload>
            {
                new ProductDetailUpdateRequestPayload
                {
                    Name = null,
                    Value = null,
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            }
        };
        
        //Act
        var results = _validator.Validate(command);
        
        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(8);
    }
    
    [Fact]
    internal void ShouldNegateInvalidCommandWithNullValuesConsideringListAsNull()
    {
            //Arrange
        var command = new UpdateProductCommand
        {
            ProductId = Guid.NewGuid(),
            BrandId = Guid.NewGuid(),
            Name = "nouveau GPU",
            Description = "Runs great on linux",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "nv-gpu",
            Price = 100.00m,
            StockUnitCount = 9,
            TagsIds = null,
            Measurements = null,
            TechnicalDetails = null,
            OtherDetails = null
        };
        
        //Act
        var results = _validator.Validate(command);
        
        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(4);
    }
}