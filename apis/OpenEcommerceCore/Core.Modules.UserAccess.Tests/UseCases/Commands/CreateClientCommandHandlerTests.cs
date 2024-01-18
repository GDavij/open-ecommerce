using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.UserAccess.Application.Messaging.Commands.CreateClient;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Entities;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.UserAccess.Tests.UseCases.Commands;

public class CreateClientCommandHandlerTests
{
    private readonly IAppConfigService _appConfigService;
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    private readonly ICreateClientCommandHandler _command;

    public CreateClientCommandHandlerTests()
    {
        _appConfigService = Substitute.For<IAppConfigService>();
        _dbContext = Substitute.For<IUserAccessContext>();
        _securityService = new SecurityService(_appConfigService);
        _command = new CreateClientCommandHandler(_dbContext, _securityService);
    }

    [Fact]
    internal async Task ShouldCreateClientIntoModuleDatabase()
    {
        // Arrange
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        IQueryable<Client> clientQueryable = new List<Client>().AsQueryable();
        DbSet<Client> clientMockedDbSet = clientQueryable.BuildMockDbSet();

        _dbContext.Clients
            .Returns(clientMockedDbSet);

        var request = new CreateClientCommand(
            Guid.NewGuid(),
            "unitTestCreateClient@email.com",
            "unit-test-password");
        
        var consumerRequest = Substitute.For<ConsumeContext<CreateClientCommand>>();
        
        consumerRequest.Message
            .Returns(request);

        Client createdClient = null;

        _dbContext.Clients.Add(Arg.Do<Client>(c => createdClient = c));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        createdClient
            .Should()
            .NotBeNull();

        createdClient.ClientModuleId
            .Should()
            .Be(request.ClientModuleId);

        createdClient.Email
            .Should()
            .Be(request.Email);
    }
}