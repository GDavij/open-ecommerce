using System;
using Core.Modules.Stock.Application.Http.Commands.CreateMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.CreateMeasureUnit;

public class CreateMeasureUnitCommandValidatorTests
{
    private readonly AbstractValidator<CreateMeasureUnitCommand> _validator;

    public CreateMeasureUnitCommandValidatorTests()
    {
        _validator = new CreateMeasureUnitCommandValidator();
    }

    [Fact]
    internal void ShouldAcceptValidCommand()
    {
        //Arrange
        var command = new CreateMeasureUnitCommand
        {
            Name = "Kilogram",
            ShortName = "Kilo",
            Symbol = "Kg"
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
        var command = new CreateMeasureUnitCommand
        {
            Name = String.Empty,
            ShortName = null,
            Symbol = null
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
    internal void ShouldNegateInvalidCommandWithValuesHigherThanMaximumValue()
    {
        //Arrange
        var command = new CreateMeasureUnitCommand
        {
            Name = new string('*', 129),
            ShortName = new string('*', 41),
            Symbol = new string('*', 5)
        };
        
        //Act
        var results = _validator.Validate(command);
        
        //Assert
        results.IsValid
            .Should()
            .BeFalse();

        results.Errors.Count
            .Should()
            .Be(3);
    }
}