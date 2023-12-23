using System;
using Core.Modules.Stock.Application.Http.Commands.RemoveImageFromProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.RemoveImageFromProduct;

public class RemoveImageFromProductCommandValidatorTests
{
    private readonly AbstractValidator<RemoveImageFromProductCommand> _validator;

    public RemoveImageFromProductCommandValidatorTests()
    {
        _validator = new RemoveImageFromProductCommandValidator();
    }

    [Fact]
    internal void ShouldAcceptValidCommand()
    {
        //Arrange
        var command = new RemoveImageFromProductCommand
        {
            Id = Guid.NewGuid()
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

    [Theory]
    [InlineData(default)]
    [InlineData(null)]
    internal void ShouldNegateInvalidCommandWithEmptyValues(Guid emptyValue)
    {
        //Arrange
        var command = new RemoveImageFromProductCommand
        {
            Id = emptyValue
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