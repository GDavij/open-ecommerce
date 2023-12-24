using System;
using Core.Modules.Stock.Application.Http.Commands.CreateBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.CreateBrand;

public class CreateBrandCommandValidatorTests
{
    private readonly AbstractValidator<CreateBrandCommand> _validator;

    public CreateBrandCommandValidatorTests()
    {
        _validator = new CreateBrandCommandValidator();
    }

    [Fact]
    internal void ShouldAcceptValidCommand()
    {
        //Arrange
        var command = new CreateBrandCommand
        {
            Name = "brand-1",
            Description = "sells computers"
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
    internal void ShouldNegateCommandWithEmptyValues()
    {
        //Arrange
        var command = new CreateBrandCommand
        {
            Name = String.Empty,
            Description = default
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
    
    [Fact]
    internal void ShouldNegateCommandWithValuesHigherThanMaximumLimit()
    {
        //Arrange
        var command = new CreateBrandCommand
        {
            Name = new string('*', 256),
            Description = new string('*', 513)
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
}