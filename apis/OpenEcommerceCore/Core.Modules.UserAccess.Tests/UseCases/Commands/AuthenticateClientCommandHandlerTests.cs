using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.Models;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.UserAccess.Application.Messaging.Commands.AuthenticateClient;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using FluentAssertions;
using MassTransit;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.UserAccess.Tests.UseCases.Commands;

public class AuthenticateClientCommandHandlerTests
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    private readonly IAppConfigService _appConfigService;
    private readonly IUserAccessDateTimeProvider _userAccessDateTimeProvider;
    private readonly IAuthenticateClientCommandHandler _command;

    public AuthenticateClientCommandHandlerTests()
    {
        _dbContext = Substitute.For<IUserAccessContext>();
        _appConfigService = Substitute.For<IAppConfigService>();
        _securityService = new SecurityService(_appConfigService);
        _userAccessDateTimeProvider = Substitute.For<IUserAccessDateTimeProvider>();
        _command = new AuthenticateClientCommandHandler(_securityService, _dbContext, _userAccessDateTimeProvider);
    }
    
    [Fact]
    internal async Task ShouldAuthenticateValidClient()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        byte[] securityKey = _securityService.GenerateSecurityKey();

        string rawClientPassword = "unit-tests-secure-password";
        byte[] derivedPassword = await _securityService.DerivePassword(
            rawClientPassword,
            securityKey,
            cancellationToken);
        
        var existentClient = Client.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "unittests@dev.com",
            derivedPassword,
            securityKey
            );
        
        // Mock Database Context
        IQueryable<Client> mockClientQueryable = new List<Client>
        {
            existentClient
        }.AsQueryable();

        var mockClientDbSet = mockClientQueryable.BuildMockDbSet();
        _dbContext.Clients.Returns(mockClientDbSet);
       
        
        // Create a mock dateTime 
        _userAccessDateTimeProvider.UtcNowOffset
            .Returns(DateTimeOffset.UtcNow);
        
        // Create a Valid Token
        var validToken = Token.Create(
            existentClient.Id,
            existentClient.Password,
            ETokenType.Client,
            TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));
        
        var validEncodedToken = _securityService.EncodeToken(validToken);

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateClientCommand>>();
        var request = new AuthenticateClientCommand(validEncodedToken);

        consumerRequest.Message
            .Returns(request);
        
        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        var expectedIdentity = Identity.Create(existentClient.ClientModuleId);

        await consumerRequest.ReceivedWithAnyArgs(1)
            .RespondAsync(AuthenticationResult.IsAuthenticatedWithIdentity(expectedIdentity));

        authenticationResult!.IsAuthenticated
            .Should()
            .BeTrue();
        
        authenticationResult.Identity!.Id
            .Should()
            .Be(existentClient.ClientModuleId);
    }
    
    [Fact]
    internal async Task ShouldNotAuthenticateClientWithInvalidPassword()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        byte[] securityKey = _securityService.GenerateSecurityKey();

        string rawClientPassword = "unit-tests-secure-password";
        byte[] derivedPassword = await _securityService.DerivePassword(
            rawClientPassword,
            securityKey,
            cancellationToken);
        
        var existentClient = Client.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "unittests@dev.com",
            derivedPassword,
            securityKey
            );
        
        // Mock Database Context
        IQueryable<Client> mockClientQueryable = new List<Client>
        {
            existentClient
        }.AsQueryable();

        var mockClientDbSet = mockClientQueryable.BuildMockDbSet();
        _dbContext.Clients.Returns(mockClientDbSet);
        
        // Mock DateTime
        _userAccessDateTimeProvider.UtcNowOffset
            .Returns(DateTimeOffset.UtcNow);
        
        // Create a Invalid Token
        string clientRawWrongPassword = "unit-tests-wrong-password";
        byte[] clientWrongDerivedPassword = await _securityService.DerivePassword(
            clientRawWrongPassword, 
            existentClient.SecurityKey,
            cancellationToken);
        
        var validToken = Token.Create(
            existentClient.Id,
            clientWrongDerivedPassword,
            ETokenType.Client,
            TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));
        
        var validEncodedToken = _securityService.EncodeToken(validToken);

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateClientCommand>>();
        var request = new AuthenticateClientCommand(validEncodedToken);

        consumerRequest.Message
            .Returns(request);
        
        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        authenticationResult!.IsAuthenticated
            .Should()
            .BeFalse();

        authenticationResult.Identity
            .Should()
            .BeNull();
    }
    
    [Fact]
    internal async Task ShouldNotAuthenticateClientWithNotExistentId()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        byte[] securityKey = _securityService.GenerateSecurityKey();

        string rawClientPassword = "unit-tests-secure-password";
        byte[] derivedPassword = await _securityService.DerivePassword(
            rawClientPassword,
            securityKey,
            cancellationToken);
        
        var existentClient = Client.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "unittests@dev.com",
            derivedPassword,
            securityKey
            );
        
        // Mock Database Context
        IQueryable<Client> mockClientQueryable = new List<Client>
        {
            existentClient
        }.AsQueryable();

        var mockClientDbSet = mockClientQueryable.BuildMockDbSet();
        _dbContext.Clients.Returns(mockClientDbSet);
        
        // Mock DateTime
        _userAccessDateTimeProvider.UtcNowOffset
            .Returns(DateTimeOffset.UtcNow);
        
        // Create a Invalid Token

        Guid clientWrongId = Guid.NewGuid();
        var validToken = Token.Create(
            clientWrongId,
            existentClient.Password,
            ETokenType.Client,
            TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));
        
        var validEncodedToken = _securityService.EncodeToken(validToken);

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateClientCommand>>();
        var request = new AuthenticateClientCommand(validEncodedToken);

        consumerRequest.Message
            .Returns(request);
        
        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Mock DateTime
        _userAccessDateTimeProvider.UtcNowOffset
            .Returns(DateTimeOffset.UtcNow);
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        authenticationResult!.IsAuthenticated
            .Should()
            .BeFalse();

        authenticationResult.Identity
            .Should()
            .BeNull();
    }
    
    
    [Fact]
    internal async Task ShouldNotAuthenticateClientWithInvalidToken()
    {
        // Arrange
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");
        
        // Create a Invalid Token
        var invalidEncodedToken ="wrong.fake.tokenjwt";

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateClientCommand>>();
        var request = new AuthenticateClientCommand(invalidEncodedToken);

        consumerRequest.Message
            .Returns(request);
        
        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        authenticationResult!.IsAuthenticated
            .Should()
            .BeFalse();

        authenticationResult.Identity
            .Should()
            .BeNull();
    }
    
    [Fact]
    internal async Task ShouldNotAuthenticateClientWithExpiredToken()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        byte[] securityKey = _securityService.GenerateSecurityKey();

        string rawClientPassword = "unit-tests-secure-password";
        byte[] derivedPassword = await _securityService.DerivePassword(
            rawClientPassword,
            securityKey,
            cancellationToken);
        
        var existentClient = Client.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "unittests@dev.com",
            derivedPassword,
            securityKey
            );
        
        // Mock Database Context
        IQueryable<Client> mockClientQueryable = new List<Client>
        {
            existentClient
        }.AsQueryable();

        var mockClientDbSet = mockClientQueryable.BuildMockDbSet();
        _dbContext.Clients.Returns(mockClientDbSet);
        
        // Mock DateTime
        _userAccessDateTimeProvider.UtcNowOffset
            .Returns(DateTimeOffset.UtcNow);
        
        // Create a Valid Token
        var validToken = Token.Create(
            existentClient.Id,
            existentClient.Password,
            ETokenType.Client,
            TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));
        
        var validEncodedToken = _securityService.EncodeToken(validToken);

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateClientCommand>>();
        var request = new AuthenticateClientCommand(validEncodedToken);

        consumerRequest.Message
            .Returns(request);
        
        
        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Mock DateTime - Expired Token Time
        _userAccessDateTimeProvider.UtcNowOffset
            .Returns(DateTimeOffset.UtcNow.AddDays(1));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        var expectedIdentity = Identity.Create(existentClient.ClientModuleId);

        await consumerRequest.ReceivedWithAnyArgs(1)
            .RespondAsync(AuthenticationResult.IsAuthenticatedWithIdentity(expectedIdentity));

        authenticationResult!.IsAuthenticated
            .Should()
            .BeFalse();

        authenticationResult.Identity
            .Should()
            .BeNull();
    }
}