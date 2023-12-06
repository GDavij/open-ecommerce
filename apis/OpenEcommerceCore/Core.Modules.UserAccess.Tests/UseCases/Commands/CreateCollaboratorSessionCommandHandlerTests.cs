using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaboratorSession;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.UserAccess.Tests.UseCases.Commands;

public class CreateCollaboratorSessionCommandHandlerTests
{
    private readonly IAppConfigService _appConfigService;
    private readonly ISecurityService _securityService;
    private readonly IUserAccessContext _dbContext;
    private readonly ICreateCollaboratorSessionCommandHandler _command;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateCollaboratorSessionCommandHandlerTests()
    {
        _appConfigService = Substitute.For<IAppConfigService>();
        _securityService = new SecurityService(_appConfigService);
        _dbContext = Substitute.For<IUserAccessContext>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _command = new CreateCollaboratorSessionCommandHandler(_dbContext, _securityService, _dateTimeProvider);
    }

    [Fact]
    internal async Task ShouldCreateCollaboratorSession()
    {
        // Arrange
        CancellationToken cancellationToken = default;
        
        // Mock AppSettings Variables
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");

        string systemValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");
        
        string collaboratorEmail = "dev@unittests.com";
        string rawCollaboratorPassword = "secure-unit-test-password";

        byte[] collaboratorSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedCollaboratorPassword = await _securityService.DerivePassword(
            rawCollaboratorPassword,
            collaboratorSecurityKey,
            cancellationToken);

        Collaborator mockedDatabaseCollaborator = Collaborator.Create(
            Guid.NewGuid(),
            default,
            collaboratorEmail,
            derivedCollaboratorPassword,
            collaboratorSecurityKey,
            ECollaboratorSector.Stock);

        CreateCollaboratorSessionCommand request = new CreateCollaboratorSessionCommand(collaboratorEmail, rawCollaboratorPassword);
        
        // Mock Database Return 
        IQueryable<Collaborator> mockCollaboratorQueryable = new List<Collaborator>
        {
            mockedDatabaseCollaborator
        }.AsQueryable();

        var mockCollaboratorDbSet = mockCollaboratorQueryable.BuildMockDbSet();
        _dbContext.Collaborators.Returns(mockCollaboratorDbSet);
        
        
        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _dateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);
        
        // Act
        var result = await _command.Handle(request, cancellationToken);

        // Assert
        Token expectedToken = Token.Create(
            mockedDatabaseCollaborator.Id,
            mockedDatabaseCollaborator.Password,
            ETokenType.Collaborator,
            TokenExpiration.OneDayFromNow(_dateTimeProvider));

        string expectedEncodedToken = _securityService.EncodeToken(expectedToken);

        result.IsValid
            .Should()
            .BeTrue();

        result.Code
            .Should()
            .Be(HttpStatusCode.OK);

        result.Result!.Token
            .Should()
            .Be(expectedEncodedToken);
    }

    [Fact]
    internal async Task ShouldNotCreateCollaboratorSessionForDeletedCollaborator()
    {
        // Arrange
        CancellationToken cancellationToken = default;

        // Mock AppSettings Variables
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");

        string systemValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        string collaboratorEmail = "dev@unittests.com";
        string rawCollaboratorPassword = "secure-unit-test-password";

        byte[] collaboratorSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedCollaboratorPassword = await _securityService.DerivePassword(
            rawCollaboratorPassword,
            collaboratorSecurityKey,
            cancellationToken);

        Collaborator mockedDatabaseCollaborator = CreateDeletedCollaborator(
            Guid.NewGuid(),
            default,
            collaboratorEmail,
            derivedCollaboratorPassword,
            collaboratorSecurityKey,
            ECollaboratorSector.Stock);

        CreateCollaboratorSessionCommand request = new CreateCollaboratorSessionCommand(collaboratorEmail, rawCollaboratorPassword);

        // Mock Database Return 
        IQueryable<Collaborator> mockCollaboratorQueryable = new List<Collaborator>
        {
            mockedDatabaseCollaborator
        }.AsQueryable();

        var mockCollaboratorDbSet = mockCollaboratorQueryable.BuildMockDbSet();
        _dbContext.Collaborators.Returns(mockCollaboratorDbSet);


        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _dateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);

        // Act
        var result = await _command.Handle(request, cancellationToken);

        // Assert
        result.IsValid
            .Should()
            .BeFalse();

        result.Code
            .Should()
            .Be(HttpStatusCode.NotFound);

        result.Result
            .Should()
            .BeNull();
    }
    
    [Fact]
    internal async Task ShouldNotCreateCollaboratorSessionWithNotExistentEmail()
    {
        // Arrange
        CancellationToken cancellationToken = default;

        // Mock AppSettings Variables
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");

        string systemValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        string collaboratorEmail = "dev@unittests.com";
        string rawCollaboratorPassword = "secure-unit-test-password";

        byte[] collaboratorSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedCollaboratorPassword = await _securityService.DerivePassword(
            rawCollaboratorPassword,
            collaboratorSecurityKey,
            cancellationToken);

        Collaborator mockedDatabaseCollaborator = Collaborator.Create(
            Guid.NewGuid(),
            default,
            collaboratorEmail,
            derivedCollaboratorPassword,
            collaboratorSecurityKey,
            ECollaboratorSector.Stock);

        string collaboratorWrongEmail = "dev@unittests.com.br";
        CreateCollaboratorSessionCommand request = new CreateCollaboratorSessionCommand(collaboratorWrongEmail, rawCollaboratorPassword);

        // Mock Database Return 
        IQueryable<Collaborator> mockCollaboratorQueryable = new List<Collaborator>
        {
            mockedDatabaseCollaborator
        }.AsQueryable();

        var mockCollaboratorDbSet = mockCollaboratorQueryable.BuildMockDbSet();
        _dbContext.Collaborators.Returns(mockCollaboratorDbSet);


        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _dateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);

        // Act
        var result = await _command.Handle(request, cancellationToken);

        // Assert
        result.IsValid
            .Should()
            .BeFalse();

        result.Code
            .Should()
            .Be(HttpStatusCode.NotFound);

        result.Result
            .Should()
            .BeNull();
    }
    
    [Fact]
    internal async Task ShouldNotCreateCollaboratorSessionWithWrongPassword()
    {
        // Arrange
        CancellationToken cancellationToken = default;

        // Mock AppSettings Variables
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");

        string systemValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(systemValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        string collaboratorEmail = "dev@unittests.com";
        string rawCollaboratorPassword = "secure-unit-test-password";

        byte[] collaboratorSecurityKey = _securityService.GenerateSecurityKey();
        byte[] derivedCollaboratorPassword = await _securityService.DerivePassword(
            rawCollaboratorPassword,
            collaboratorSecurityKey,
            cancellationToken);

        Collaborator mockedDatabaseCollaborator = Collaborator.Create(
            Guid.NewGuid(),
            default,
            collaboratorEmail,
            derivedCollaboratorPassword,
            collaboratorSecurityKey,
            ECollaboratorSector.Stock);

        string collaboratorWrongRawPassword = "secure-unit-test-wrong-password";
        CreateCollaboratorSessionCommand request = new CreateCollaboratorSessionCommand(collaboratorEmail, collaboratorWrongRawPassword);

        // Mock Database Return 
        IQueryable<Collaborator> mockCollaboratorQueryable = new List<Collaborator>
        {
            mockedDatabaseCollaborator
        }.AsQueryable();

        var mockCollaboratorDbSet = mockCollaboratorQueryable.BuildMockDbSet();
        _dbContext.Collaborators.Returns(mockCollaboratorDbSet);


        // Mock DateTime for Token Equality
        DateTimeOffset testEnvironmentDateTimeOffset = DateTimeOffset.UtcNow;
        _dateTimeProvider.UtcNowOffset.Returns(testEnvironmentDateTimeOffset);

        // Act
        var result = await _command.Handle(request, cancellationToken);

        // Assert
        result.IsValid
            .Should()
            .BeFalse();

        result.Code
            .Should()
            .Be(HttpStatusCode.BadRequest);

        result.Result
            .Should()
            .BeNull();
    }

    private Collaborator CreateDeletedCollaborator(
        Guid id,
        Guid moduleCollaboratorId,
        string email,
        byte[] password,
        byte[] securityKey,
        ECollaboratorSector sector)
    {
        var collaborator = Collaborator.Create(
            id,
            moduleCollaboratorId,
            email,
            password,
            securityKey,
            sector);

        collaborator.Deleted = true;
        return collaborator;
    }
}