using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.Models;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Application.UseCases.Commands.AuthenticateClient;
using Core.Modules.UserAccess.Application.UseCases.Commands.AuthenticateCollaboratorForSector;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.UserAccess.Tests.UseCases.Commands;

public class AuthenticateCollaboratorForSectorCommandHandlerTests
{
     private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    private readonly IAppConfigService _appConfigService;
    private readonly IUserAccessDateTimeProvider _userAccessDateTimeProvider;
    private readonly IAuthenticateCollaboratorForSectorCommandHandler _command;

    public AuthenticateCollaboratorForSectorCommandHandlerTests()
    {
        _dbContext = Substitute.For<IUserAccessContext>();
        _appConfigService = Substitute.For<IAppConfigService>();
        _securityService = new SecurityService(_appConfigService);
        _userAccessDateTimeProvider = Substitute.For<IUserAccessDateTimeProvider>();
        _command = new AuthenticateCollaboratorForSectorCommandHandler(_securityService, _dbContext, _userAccessDateTimeProvider);
    }

    [Fact]
    internal async Task ShouldAuthenticateValidCollaboratorForItsModule()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");
        string rawCollaboratorPassword = "secure-unit-tests-collaborator-password";
        
        byte[] securityKey = _securityService.GenerateSecurityKey();
        byte[] derivedPassword = await _securityService.DerivePassword(
            rawCollaboratorPassword,
            securityKey,
            cancellationToken);
        
        ECollaboratorSector collaboratorSector = ECollaboratorSector.Stock;

        // Mock Database 
        var databaseCollaborator = Collaborator.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "collaboratoremail@unittests.com",
            derivedPassword,
            securityKey,
            collaboratorSector);

        IQueryable<Collaborator> mockCollaboratorQueryable = new List<Collaborator>
        {
            databaseCollaborator
        }.AsQueryable();

        DbSet<Collaborator> mockCollaboratorDbSet = mockCollaboratorQueryable.BuildMockDbSet();

        _dbContext.Collaborators.Returns(mockCollaboratorDbSet);
        
        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _userAccessDateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);
        
        var validToken = Token.Create(
            databaseCollaborator.Id,
            databaseCollaborator.Password,
            ETokenType.Collaborator,
            TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));
        
        var validEncodedToken = _securityService.EncodeToken(validToken);

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateCollaboratorForSectorCommand>>();
        var request = new AuthenticateCollaboratorForSectorCommand(validEncodedToken, databaseCollaborator.Sector);
        
        consumerRequest.Message
            .Returns(request);

        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        var expectedIdentity = Identity.Create(databaseCollaborator.CollaboratorModuleId);
        
        await consumerRequest.ReceivedWithAnyArgs(1)
            .RespondAsync(AuthenticationResult.IsAuthenticatedWithIdentity(expectedIdentity));

        authenticationResult!.IsAuthenticated
            .Should()
            .BeTrue();

        authenticationResult.Identity!.Id
            .Should()
            .Be(databaseCollaborator.CollaboratorModuleId);
    }
    
    [Fact]
    internal async Task ShouldAuthenticateValidCollaboratorForItsModuleWithInvalidPassword()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");
        string rawCollaboratorPassword = "secure-unit-tests-collaborator-password";
        
        byte[] securityKey = _securityService.GenerateSecurityKey();
        byte[] derivedPassword = await _securityService.DerivePassword(
            rawCollaboratorPassword,
            securityKey,
            cancellationToken);
        
        ECollaboratorSector collaboratorSector = ECollaboratorSector.Stock;

        // Mock Database 
        var databaseCollaborator = Collaborator.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "collaboratoremail@unittests.com",
            derivedPassword,
            securityKey,
            collaboratorSector);

        IQueryable<Collaborator> mockCollaboratorQueryable = new List<Collaborator>
        {
            databaseCollaborator
        }.AsQueryable();

        DbSet<Collaborator> mockCollaboratorDbSet = mockCollaboratorQueryable.BuildMockDbSet();

        _dbContext.Collaborators.Returns(mockCollaboratorDbSet);
        
        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _userAccessDateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);

        var invalidRawPassword = "secret-invalid-password";
        byte[] invalidDerivedPassword = await _securityService.DerivePassword(
            invalidRawPassword,
            databaseCollaborator.SecurityKey,
            cancellationToken);
        
        var validToken = Token.Create(
            databaseCollaborator.Id,
            invalidDerivedPassword,
            ETokenType.Collaborator,
            TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));
        
        var validEncodedToken = _securityService.EncodeToken(validToken);

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateCollaboratorForSectorCommand>>();
        var request = new AuthenticateCollaboratorForSectorCommand(validEncodedToken, databaseCollaborator.Sector);
        
        consumerRequest.Message
            .Returns(request);

        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        var expectedIdentity = Identity.Create(databaseCollaborator.CollaboratorModuleId);
        
        await consumerRequest.ReceivedWithAnyArgs(1)
            .RespondAsync(AuthenticationResult.NotAuthenticated());

        authenticationResult!.IsAuthenticated
            .Should()
            .BeFalse();
    }
    
    [Fact]
    internal async Task ShouldAuthenticateValidCollaboratorForItsModuleWithInvalidToken()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");
        
        string rawCollaboratorPassword = "secure-unit-tests-collaborator-password";
        
        byte[] securityKey = _securityService.GenerateSecurityKey();
        byte[] derivedPassword = await _securityService.DerivePassword(
            rawCollaboratorPassword,
            securityKey,
            cancellationToken);
        
        ECollaboratorSector collaboratorSector = ECollaboratorSector.Stock;

        // Mock Database 
        var databaseCollaborator = Collaborator.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "collaboratoremail@unittests.com",
            derivedPassword,
            securityKey,
            collaboratorSector);

        IQueryable<Collaborator> mockCollaboratorQueryable = new List<Collaborator>
        {
            databaseCollaborator
        }.AsQueryable();

        DbSet<Collaborator> mockCollaboratorDbSet = mockCollaboratorQueryable.BuildMockDbSet();

        _dbContext.Collaborators.Returns(mockCollaboratorDbSet);
        
        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _userAccessDateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);


        var invalidEncodedToken = "invalid-jwt";

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateCollaboratorForSectorCommand>>();
        var request = new AuthenticateCollaboratorForSectorCommand(invalidEncodedToken, databaseCollaborator.Sector);
        
        consumerRequest.Message
            .Returns(request);

        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        var expectedIdentity = Identity.Create(databaseCollaborator.CollaboratorModuleId);
        
        await consumerRequest.ReceivedWithAnyArgs(1)
            .RespondAsync(AuthenticationResult.NotAuthenticated());

        authenticationResult!.IsAuthenticated
            .Should()
            .BeFalse();
    }
    
    [Fact]
    internal async Task ShouldAuthenticateValidCollaboratorForItsModuleWithNotExistentId()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");
        
        string rawCollaboratorPassword = "secure-unit-tests-collaborator-password";
        
        byte[] securityKey = _securityService.GenerateSecurityKey();
        byte[] derivedPassword = await _securityService.DerivePassword(
            rawCollaboratorPassword,
            securityKey,
            cancellationToken);
        
        ECollaboratorSector collaboratorSector = ECollaboratorSector.Stock;

        // Mock Database 
        var databaseCollaborator = Collaborator.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "collaboratoremail@unittests.com",
            derivedPassword,
            securityKey,
            collaboratorSector);

        IQueryable<Collaborator> mockCollaboratorQueryable = new List<Collaborator>
        {
            databaseCollaborator
        }.AsQueryable();

        DbSet<Collaborator> mockCollaboratorDbSet = mockCollaboratorQueryable.BuildMockDbSet();

        _dbContext.Collaborators.Returns(mockCollaboratorDbSet);
        
        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _userAccessDateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);
        
        Guid invalidCollaboratorId = Guid.NewGuid();
        
        var token = Token.Create(
            invalidCollaboratorId,
            databaseCollaborator.Password,
            ETokenType.Collaborator,
            TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));

        var invalidEncodedToken = _securityService.EncodeToken(token);

        var consumerRequest = Substitute.For<ConsumeContext<AuthenticateCollaboratorForSectorCommand>>();
        var request = new AuthenticateCollaboratorForSectorCommand(invalidEncodedToken, databaseCollaborator.Sector);
        
        consumerRequest.Message
            .Returns(request);

        // Necessary to assign so the compiler doesn't complain 
        AuthenticationResult? authenticationResult = null;
        await consumerRequest.RespondAsync(Arg.Do<AuthenticationResult>(result => authenticationResult = result));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        var expectedIdentity = Identity.Create(databaseCollaborator.CollaboratorModuleId);
        
        await consumerRequest.ReceivedWithAnyArgs(1)
            .RespondAsync(AuthenticationResult.NotAuthenticated());

        authenticationResult!.IsAuthenticated
            .Should()
            .BeFalse();
    }
}