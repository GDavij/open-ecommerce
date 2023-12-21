using System;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.Constants;
using Core.Modules.Stock.Application.Http.Commands.AddImageToProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.AddImageToProduct;

public class AddImageToProductCommandValidatorTests
{
    private readonly AbstractValidator<AddImageToProductCommand> _validator;

    public AddImageToProductCommandValidatorTests()
    {
        _validator = new AddImageToProductCommandValidator();
    }
    
    [Fact]
    internal void ShouldAcceptValidCommand()
    {
        //Arrange
        var mockedFileImage = Substitute.For<IFormFile>();

        mockedFileImage.ContentType
            .Returns("image/jpeg");

        mockedFileImage.Length
            .Returns(2097121);

        var command = new AddImageToProductCommand
        {
            ProductId = Guid.NewGuid(),
            Description = "Drill v4 480RPM Descriptive image",
            ImageFile = mockedFileImage
        };
        
        
        //Act
        var result = _validator.Validate(command);
        
        //Assert
        result.IsValid
            .Should()
            .BeTrue();

        result.Errors.Count
            .Should()
            .Be(0);
    }

    [Fact]
    internal void ShouldNegateCommandWithEmptyValues()
    {
        //Arrange
        var command = new AddImageToProductCommand
        {
            ProductId = default,
            Description = default,
            ImageFile = null
        };
        
        //Act
        var result = _validator.Validate(command);

        //Assert
        result.IsValid
            .Should()
            .BeFalse();

        result.Errors.Count
            .Should()
            .Be(3);
    }

    [Fact]
    internal void ShouldNegateCommandWithValuesLowerThanMinimalLimit()
    {
        //Arrange
        var mockedFileImage = Substitute.For<IFormFile>();

        mockedFileImage.ContentType
            .Returns("image/jpeg");

        mockedFileImage.Length
            .Returns(-1);

        var command = new AddImageToProductCommand
        {
            ProductId = Guid.NewGuid(),
            Description = "1234567",
            ImageFile = mockedFileImage
        };
        
        //Act
        var results = _validator.Validate(command);

        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(2);
    }

    [Fact]
    internal void ShouldNegateCommandWithValuesHigherThanMaxLimit()
    {
        //Arrange
        var mockedFileImage = Substitute.For<IFormFile>();

        mockedFileImage.ContentType
            .Returns("image/jpeg");

        mockedFileImage.Length
            .Returns((MemoryMeasure.Megabytes * 2) + 1);

        var command = new AddImageToProductCommand
        {
            ProductId = Guid.NewGuid(),
            Description = new string('*', 256),
            ImageFile = mockedFileImage
        };
        
        //Act
        var results = _validator.Validate(command);

        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(2);
    }

    [Fact]
    internal void ShouldNegateCommandWithInvalidMimeType()
    {
        //Arrange
        var mockedFileImage = Substitute.For<IFormFile>();

        mockedFileImage.ContentType
            .Returns("image/gif");

        mockedFileImage.Length
            .Returns(0048576);

        var command = new AddImageToProductCommand
        {
            ProductId = Guid.NewGuid(),
            Description = "Drill Image with Specifications of product",
            ImageFile = mockedFileImage
        };
        
        //Act
        var results = _validator.Validate(command);

        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(1);
    }
}