using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Modules.Stock.Application.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Product;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.CreateProduct;


public class CreateProductCommandValidatorTests
{
    private readonly AbstractValidator<CreateProductCommand> _validator;

    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact]
    public void ShouldAcceptValidCommand()
    {
        //Arrange
        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");

        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Upc = "123456789123",
            Sku = "brand-1-m1",
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = Brand.Create("brand-1", "sells-computers").Id,
            TagsIds = new List<Guid>
            {
                ProductTag.Create("processor-2x").Id,
                ProductTag.Create("linear-algebra-processor").Id
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
                    MeasureUnitId = null
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Has Support to brand-1 Ai TPU runner system",
                    Value = "False",
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            },
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
    public void ShouldNegateInvalidCommandWithNullValuesConsideringListAsNull()
    {
        //Arrange
        CreateProductCommand command = new CreateProductCommand
        {
            Name = null,
            Description = null,
            Ean = null,
            Upc = null,
            Sku = null,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = Brand.Create("brand-1", "sells-computers").Id, // Will not be parseable as null
            TagsIds = null,
            Measurements = null,
            TechinicalDetails = null,
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
            .Be(6);
    }

    [Fact]
    public void ShouldNegateInvalidCommandWithNullValuesConsideringInnerListValuesAsNull()
    {
        //Arrange
        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");

        CreateProductCommand command = new CreateProductCommand
        {
            Name = null,
            Description = null,
            Ean = null,
            Upc = null,
            Sku = null,
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = Brand.Create("brand-1", "sells-computers").Id,
            TagsIds = new List<Guid>
            {
                ProductTag.Create("processor-2x").Id, // Cannot parse null to Guid
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = null,
                    Value = null,
                    ShowOrder = 1,
                    MeasureUnitId = null
                },
                new ProductDetailRequestPayload
                {
                    Name = null,
                    Value = null,
                    ShowOrder = 2,
                    MeasureUnitId = null
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = null,
                    Value = null,
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = null,
                    Value = null,
                    ShowOrder = 1,
                    MeasureUnitId = null
                }
            },
        };
        //Act
        var results = _validator.Validate(command);

        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(10);
    }

    [Fact]
    public void ShouldNegateInvalidCommandWithEmptyValues()
    {
        //Arrange
        CreateProductCommand command = new CreateProductCommand
        {
            Name = String.Empty,
            Description = default,
            Ean = String.Empty,
            Upc = default,
            Sku = default,
            Price = default,
            StockUnitCount = default,
            BrandId = Guid.Empty,
            TagsIds = new List<Guid>
            {
                Guid.Empty,
                Guid.Empty
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = String.Empty,
                    Value = String.Empty,
                    ShowOrder = default,
                    MeasureUnitId = Guid.Empty
                },
                new ProductDetailRequestPayload
                {
                    Name = String.Empty,
                    Value = String.Empty,
                    ShowOrder = default,
                    MeasureUnitId = Guid.Empty
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = String.Empty,
                    Value = String.Empty,
                    ShowOrder = default,
                    MeasureUnitId = Guid.Empty
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = String.Empty,
                    Value = String.Empty,
                    ShowOrder = default,
                    MeasureUnitId = Guid.Empty
                }
            },
        };

        //Act
        var results = _validator.Validate(command);

        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(25);
    }

    [Fact]
    public void ShouldNegateInvalidCommandWithLessThanMinValues()
    {
        //Arrange
        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");

        CreateProductCommand command = new CreateProductCommand
        {
            Name = "",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "ean",
            Upc = "",
            Sku = "sku",
            Price = -1m,
            StockUnitCount = -1,
            BrandId = Brand.Create("brand-1", "sells-computers").Id,
            TagsIds = new List<Guid>
            {
                ProductTag.Create("processor-2x").Id,
                ProductTag.Create("linear-algebra-processor").Id
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Width",
                    Value = "1920",
                    ShowOrder = -1,
                    MeasureUnitId = pixelMeasureUnit.Id
                },
                new ProductDetailRequestPayload
                {
                    Name = "Height",
                    Value = "1080",
                    ShowOrder = -1000,
                    MeasureUnitId = megabytesMeasureUnit.Id
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Resolution",
                    Value = "8K",
                    ShowOrder = -4124,
                    MeasureUnitId = null
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = "Has Support to brand-1 Ai TPU runner system",
                    Value = "False",
                    ShowOrder = -0,
                    MeasureUnitId = null
                }
            },
        };

        //Act
        var results = _validator.Validate(command);

        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(12);
    }

    [Fact]
    public void ShouldNegateInvalidCommandWithMoreThanMaxValues()
    {
        //Arrange
        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");

        CreateProductCommand command = new CreateProductCommand
        {
            Name = new string('*', 256),
            Description = new string('*', 513),
            Ean = new string('*', 14),
            Upc = new string('*', 13),
            Sku = new string('*', 21),
            Price = 12345678912345678.123456m,
            StockUnitCount = int.MaxValue,
            BrandId = Brand.Create("brand-1", "sells-computers").Id,
            TagsIds = new List<Guid>
            {
                ProductTag.Create("processor-2x").Id,
                ProductTag.Create("linear-algebra-processor").Id
            },
            Measurements = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = new string('*', 256),
                    Value = new string('*', 256),
                    ShowOrder = int.MaxValue,
                    MeasureUnitId = pixelMeasureUnit.Id
                },
                new ProductDetailRequestPayload
                {
                    Name = new string('*', 256),
                    Value = new string('*', 256),
                    ShowOrder = int.MaxValue - 1,
                    MeasureUnitId = megabytesMeasureUnit.Id
                }
            },
            TechinicalDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = new string('*', 256),
                    Value = new string('*', 256),
                    ShowOrder = int.MaxValue,
                    MeasureUnitId = null
                }
            },
            OtherDetails = new List<ProductDetailRequestPayload>
            {
                new ProductDetailRequestPayload
                {
                    Name = new string('*', 256),
                    Value = new string('*', 256),
                    ShowOrder = int.MaxValue,
                    MeasureUnitId = null
                }
            },
        };

        //Act
        var results = _validator.Validate(command);

        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(14);
    }

    [Fact]
    public void ShouldAcceptCommandWithEmptyListsForProductDetailsAndTags()
    {
        //Arrange
        var pixelMeasureUnit = MeasureUnit.Create("Pixels", "Pixel", "px");
        var megabytesMeasureUnit = MeasureUnit.Create("MegaBytes", "MegaByte", "MB");

        CreateProductCommand command = new CreateProductCommand
        {
            Name = "computer-m1",
            Description = "computer-with-high-resolution-based-on-bsd",
            Ean = "1234567891234",
            Price = 2100m,
            StockUnitCount = 10,
            BrandId = Brand.Create("brand-1", "sells-computers").Id,
            TagsIds = new List<Guid>(),
            Measurements = new List<ProductDetailRequestPayload>(),
            TechinicalDetails = new List<ProductDetailRequestPayload>(),
            OtherDetails = new List<ProductDetailRequestPayload>()
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
}