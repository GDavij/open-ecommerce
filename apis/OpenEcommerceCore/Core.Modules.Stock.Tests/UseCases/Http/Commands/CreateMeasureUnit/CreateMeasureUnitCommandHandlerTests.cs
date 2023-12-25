using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Application.Http.Commands.CreateMeasureUnit;
using Core.Modules.Stock.Application.IntegrationEvents.MeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Exceptions.MeasureUnit;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.CreateMeasureUnit;

public class CreateMeasureUnitCommandHandlerTests
{
    private readonly string _adminDashboardBaseUrl;
    private readonly IAppConfigService _configService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IStockContext _dbContext;
    private readonly ICreateMeasureUnitCommandHandler _commandHandler;

    public CreateMeasureUnitCommandHandlerTests()
    {
        _adminDashboardBaseUrl = "https://localhost:8080/dashboard";
        
        _configService = Substitute.For<IAppConfigService>();
        _configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl")
            .Returns(_adminDashboardBaseUrl);

        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _dbContext = Substitute.For<IStockContext>();
        _commandHandler = new CreateMeasureUnitCommandHandler(_dbContext, _publishEndpoint, _configService);
    }
    
    [Fact]
    internal async Task ShouldCreateMeasureUnitForValidCommand()
    {
        //Arrange
        DbSet<MeasureUnit> measureUnitDbSet = new List<MeasureUnit>()
            .AsQueryable()
            .BuildMockDbSet();
        
        _dbContext.MeasureUnits
            .Returns(measureUnitDbSet);

        MeasureUnit createdMeasureUnit = null!;

        _dbContext.MeasureUnits.Add(Arg.Do<MeasureUnit>(m => createdMeasureUnit = m));
        
        var command = new CreateMeasureUnitCommand
        {
            Name = "Megabyte",
            ShortName = null,
            Symbol = "MB"
        };
        
        //Act
        var result = await _commandHandler.Handle(command, default);
        
        //Assert
        result.Resource
            .Should()
            .Be($"{_adminDashboardBaseUrl}/measureUnits/{createdMeasureUnit.Id}");

        await _dbContext
            .Received(1)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(1)
            .Publish(Arg.Is<MeasureUnitCreatedIntegrationEvent>(ev =>
                ev.MeasureUnit.Id == createdMeasureUnit.Id &&
                ev.MeasureUnit.Name == createdMeasureUnit.Name &&
                ev.MeasureUnit.ShortName == createdMeasureUnit.ShortName
            ));
    }

    [Fact]
    internal async Task ShouldNotCreateMeasureUnitForInvalidCommandWithSameNameOrShortnameAsExistentMeasureUnit()
    {
        //Arrange - Found Existent Name
        var existentMeasureUnit = MeasureUnit.Create("Kilogram", "Kilo", "Kg");

        DbSet<MeasureUnit> measureUnitDbSet = new List<MeasureUnit>
            {
                existentMeasureUnit
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.MeasureUnits
            .Returns(measureUnitDbSet);

        var command = new CreateMeasureUnitCommand
        {
            Name = existentMeasureUnit.Name,
            ShortName = "any-str-shortname",
            Symbol = "Kg"
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };

        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<AlreadyExistentMeasureUnitException>();
        
        //Assert 
        exception.Which.Message
            .Should()
            .Be($"Found Existent Measure Unit with same Name {existentMeasureUnit.Name} or same ShortName {existentMeasureUnit.ShortName}, Conflict Exception");

        await _dbContext
            .Received(0)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<MeasureUnitCreatedIntegrationEvent>);
        
        //Arrange - Found Existent Shortname
        command = new CreateMeasureUnitCommand
        {
            Name = "any-str-name",
            ShortName = existentMeasureUnit.ShortName,
            Symbol = "Kg"
        };

        action = async () => { await _commandHandler.Handle(command, default); };
        
        //Act
        exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<AlreadyExistentMeasureUnitException>();
        
        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found Existent Measure Unit with same Name {existentMeasureUnit.Name} or same ShortName {existentMeasureUnit.ShortName}, Conflict Exception");

        await _dbContext
            .Received(0)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<MeasureUnitCreatedIntegrationEvent>());
    }
}