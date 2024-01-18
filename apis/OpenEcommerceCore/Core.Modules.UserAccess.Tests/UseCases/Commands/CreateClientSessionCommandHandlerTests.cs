using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.UserAccess.Application.Http.Commands.CreateClientSession;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;
using IUserAccessContext = Core.Modules.UserAccess.Domain.Contracts.Contexts.IUserAccessContext;

namespace Core.Modules.UserAccess.Tests.UseCases.Commands;

public class CreateClientSessionCommandHandlerTests
{
    private readonly IAppConfigService _appConfigService;
    private readonly ISecurityService _securityService;
    private readonly IUserAccessContext _dbContext;
    private readonly ICreateClientSessionCommandHandler _command;
    private readonly IUserAccessDateTimeProvider _userAccessDateTimeProvider;

    public CreateClientSessionCommandHandlerTests()
    {
        _appConfigService = Substitute.For<IAppConfigService>();
        _securityService = new SecurityService(_appConfigService);
        _dbContext = Substitute.For<IUserAccessContext>();
        _userAccessDateTimeProvider = Substitute.For<IUserAccessDateTimeProvider>();
        _command = new CreateClientSessionCommandHandler(_dbContext, _securityService, _userAccessDateTimeProvider);
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

        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        string clientEmail = "dev@unittests.com";
        string rawClientPassword = "secure-unit-test-password";

        byte[] clientSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedClientPassword = await _securityService.DerivePassword(
            rawClientPassword,
            clientSecurityKey,
            cancellationToken);

        Client mockedDatabaseClient = Client.Create(
            Guid.NewGuid(),
            default,
            clientEmail,
            derivedClientPassword,
            clientSecurityKey);

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
        _userAccessDateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);

        // Act
        var validationResult = await _command.Handle(request, cancellationToken);

        // Assert
        Token expectedToken = Token.Create(
            mockedDatabaseClient.Id,
            mockedDatabaseClient.Password,
            ETokenType.Client,
        TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));

        var expectedEncodedToken = _securityService.EncodeToken(expectedToken);

        validationResult.IsValid
            .Should()
            .BeTrue();

        validationResult.Code
            .Should()
            .Be(HttpStatusCode.OK);

        validationResult.Result!
            .Token
            .Should()
            .Be(expectedEncodedToken);
    }

    [Fact]
    internal async Task ShouldNotCreateClientSessionWithWrongPassword()
    {
        // Arrange
        CancellationToken cancellationToken = default;

        // Mock AppSettings Variables
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");

        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        string clientEmail = "dev@unittests.com";
        string rawClientPassword = "secure-unit-test-password";

        byte[] clientSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedClientPassword = await _securityService.DerivePassword(
            rawClientPassword,
            clientSecurityKey,
            cancellationToken);

        Client mockedDatabaseClient = Client.Create(
            Guid.NewGuid(),
            default,
            clientEmail,
            derivedClientPassword,
            clientSecurityKey);

        string wrongRawClientPassword = "secure-wrong-unit-test-password";
        CreateClientSessionCommand request = new CreateClientSessionCommand(clientEmail, wrongRawClientPassword);

        // Mock Database Return 
        IQueryable<Client> mockClientQueryable = new List<Client>
        {
            mockedDatabaseClient
        }.AsQueryable();

        var mockClientDbSet = mockClientQueryable.BuildMockDbSet();
        _dbContext.Clients.Returns(mockClientDbSet);

        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _userAccessDateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);

        // Act
        var validationResult = await _command.Handle(request, cancellationToken);

        // Assert
        validationResult.IsValid
            .Should()
            .BeFalse();

        validationResult.Code
            .Should()
            .Be(HttpStatusCode.BadRequest);

        validationResult.Result
            .Should()
            .BeNull();
    }

    [Fact]
    internal async Task ShouldNotCreateClientSessionWithNotExistentEmail()
    {
        // Arrange
        CancellationToken cancellationToken = default;

        // Mock AppSettings Variables
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");

        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        string clientEmail = "dev@unittests.com";
        string rawClientPassword = "secure-unit-test-password";

        byte[] clientSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedClientPassword = await _securityService.DerivePassword(
            rawClientPassword,
            clientSecurityKey,
            cancellationToken);

        Client mockedDatabaseClient = Client.Create(
            Guid.NewGuid(),
            default,
            clientEmail,
            derivedClientPassword,
            clientSecurityKey);

        string wrongClientEmail = "dev@unittests.com.br";
        CreateClientSessionCommand request = new CreateClientSessionCommand(wrongClientEmail, rawClientPassword);

        // Mock Database Return 
        IQueryable<Client> mockClientQueryable = new List<Client>
        {
            mockedDatabaseClient
        }.AsQueryable();

        var mockClientDbSet = mockClientQueryable.BuildMockDbSet();
        _dbContext.Clients.Returns(mockClientDbSet);


        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _userAccessDateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);

        // Act
        var validationResult = await _command.Handle(request, cancellationToken);

        // Assert
        validationResult.IsValid
            .Should()
            .BeFalse();

        validationResult.Code
            .Should()
            .Be(HttpStatusCode.NotFound);

        validationResult.Result
            .Should()
            .BeNull();
    }

    [Fact]
    internal async Task ShouldNotCreateClientSessionForDeletedClient()
    {
        // Arrange
        CancellationToken cancellationToken = default;

        // Mock AppSettings Variables
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");

        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        string clientEmail = "dev@unittests.com";
        string rawClientPassword = "secure-unit-test-password";

        byte[] clientSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedClientPassword = await _securityService.DerivePassword(
            rawClientPassword,
            clientSecurityKey,
            cancellationToken);

        Client mockedDatabaseClient = CreateDeletedClient(
            Guid.NewGuid(),
            default,
            clientEmail,
            derivedClientPassword,
            clientSecurityKey);

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
        _userAccessDateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);

        // Act
        var validationResult = await _command.Handle(request, cancellationToken);

        // Assert
        validationResult.IsValid
            .Should()
            .BeFalse();

        validationResult.Code
            .Should()
            .Be(HttpStatusCode.NotFound);

        validationResult.Result
            .Should()
            .BeNull();
    }

    private Client CreateDeletedClient(Guid id, Guid clientModuleId, string email, byte[] password, byte[] securityKey)
    {
        var client = Client.Create(id, clientModuleId, email, password, securityKey);
        client.Deleted = true;
        return client;
    }
}