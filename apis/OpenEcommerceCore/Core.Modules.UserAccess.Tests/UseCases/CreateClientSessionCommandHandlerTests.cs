using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Application.UseCases.Commands.CreateClientSession;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Models;
using Core.Modules.UserAccess.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;
using IUserAccessContext = Core.Modules.UserAccess.Domain.Contracts.Contexts.IUserAccessContext;

namespace Core.Modules.UserAccess.Tests.UseCases;

public class CreateClientSessionCommandHandlerTests
{
    private readonly IAppConfigService _appConfigService;
    private readonly ISecurityService _securityService;
    private readonly IUserAccessContext _dbContext;
    private readonly ICreateClientSessionCommandHandler _command;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateClientSessionCommandHandlerTests()
    {
        _appConfigService = Substitute.For<IAppConfigService>();
        _securityService = new SecurityService(_appConfigService);
        _dbContext = Substitute.For<IUserAccessContext>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _command = new CreateClientSessionCommandHandler(_dbContext, _securityService, _dateTimeProvider);
    }
    
    [Fact]
    internal async Task ShouldCreateClientSession()
    {
        // Arrange
        CancellationToken cancellationToken = default;

        // Mock AppSettings Variables
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKey = _appConfigService.GetEnvironmentVariable("user_access::JWT_SECURITY_KEY");

        _appConfigService.GetEnvironmentVariable(secretValidationKey)
            .Returns("unit-tests-jwt-security-key");
        
        string clientEmail = "dev@unittests.com";
        string rawClientPassword = "secure-unit-test-password";

        byte[] clientSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedUserPassphrase = await _securityService.DerivePassword(
            rawClientPassword,
            clientSecurityKey,
            cancellationToken);

        Client mockedDatabaseClient = Client.Create(
            Guid.NewGuid(),
            default,
            clientEmail,
            derivedUserPassphrase,
            clientSecurityKey,
            DateTime.UtcNow,
            DateTime.UtcNow);

        CreateClientSessionCommand request = new CreateClientSessionCommand(clientEmail, rawClientPassword);
        
        // Mock Database Return 
        IQueryable<Client> mockClientQueryable = new List<Client>
        {
            mockedDatabaseClient
        }.AsQueryable();

        var mockClientDbSet = mockClientQueryable.BuildMockDbSet();
        _dbContext.Clients.Returns(mockClientDbSet);
        
        
        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _dateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);
        
        // Act
        var validationResult = await _command.Handle(request, cancellationToken);

        // Assert
        Token expectedToken = Token.Create(
            mockedDatabaseClient.Id,
            mockedDatabaseClient.Password,
            ETokenType.Client,
        testEnvironmentDateTimeOffset.AddDays(1).ToUnixTimeMilliseconds());

        var expectedEncodedToken = _securityService.EncodeToken(expectedToken);

        validationResult.IsValid
            .Should()
            .BeTrue();
        
        validationResult.Result!
            .Token
            .Should()
            .Be(expectedEncodedToken);
    }
}