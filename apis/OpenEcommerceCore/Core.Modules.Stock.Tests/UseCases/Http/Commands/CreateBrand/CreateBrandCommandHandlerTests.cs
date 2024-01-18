using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Application.Http.Commands.CreateBrand;
using Core.Modules.Stock.Domain.Constants;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Exceptions.Brand;
using Core.Modules.Stock.Domain.IntegrationEvents;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.Stock.Tests.UseCases.Http.Commands.CreateBrand;

public class CreateBrandCommandHandlerTests
{
    private readonly string _adminDashboardBaseUrl;
    private readonly IAppConfigService _configService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IStockContext _dbContext;
    private readonly ICreateBrandCommandHandler _commandHandler;


    public CreateBrandCommandHandlerTests()
    {
        _configService = Substitute.For<IAppConfigService>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _dbContext = Substitute.For<IStockContext>();

        _commandHandler = new CreateBrandCommandHandler(_dbContext, _publishEndpoint, _configService);

        _adminDashboardBaseUrl = "https://localhost:8080/dashboard";
        _configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable)
            .Returns(_adminDashboardBaseUrl);
    }

    [Theory]
    [InlineData("sells computers")]
    [InlineData(null)]
    internal async Task ShouldCreateBrandForValidCommand(string? simpleDescription)
    {
        //Arrange
        DbSet<Brand> brandDbSet = new List<Brand>()
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);

        var command = new CreateBrandCommand
        {
            Name = "brand-1",
            Description = simpleDescription
        };

        Brand createdBrand = null!;
        _dbContext.Brands.Add(Arg.Do<Brand>(b => createdBrand = b));

        //Act
        var result = await _commandHandler.Handle(command, default);

        //Assert
        result.Resource
            .Should()
            .Be($"{_adminDashboardBaseUrl}/brands/{createdBrand.Id}");

        await _dbContext
            .Received(1)
            .SaveChangesAsync(default);

        await _publishEndpoint
            .Received(1)
            .Publish(Arg.Is<BrandCreatedIntegrationEvent>(ev =>
                ev.Brand.Id == createdBrand.Id &&
                ev.Brand.Name == createdBrand.Name &&
                ev.Brand.Description == createdBrand.Description));
    }

    [Fact]
    internal async Task ShouldNotCreateBrandForAlreadyExistentBrand()
    {
        //Arrange
        var existentBrand = Brand.Create("brand-1", "sells computers");

        DbSet<Brand> brandDbSet = new List<Brand>
            {
                existentBrand
            }
            .AsQueryable()
            .BuildMockDbSet();

        _dbContext.Brands
            .Returns(brandDbSet);

        var command = new CreateBrandCommand
        {
            Name = existentBrand.Name,
            Description = "sells hardware"
        };

        Func<Task> action = async () => { await _commandHandler.Handle(command, default); };

        //Act
        var exception = await FluentActions.Invoking(action)
            .Should()
            .ThrowAsync<AlreadyExistentBrandException>();

        //Assert
        exception.Which.Message
            .Should()
            .Be($"Found an existent Brand with {command.Name} Name, conflict exception");

        await _dbContext
            .Received(0)
            .SaveChangesAsync();

        await _publishEndpoint
            .Received(0)
            .Publish(Arg.Any<BaseIntegrationEvent>());
    }
}