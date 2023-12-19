using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaborator;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace Core.Modules.UserAccess.Tests.UseCases.Commands;

public class CreateCollaboratorCommandHandlerTests
{
    private readonly IAppConfigService _appConfigService;
    private readonly ISecurityService _securityService;
    private readonly IUserAccessContext _dbContext;
    private readonly ICreateCollaboratorCommandHandler _command;

    public CreateCollaboratorCommandHandlerTests()
    {
        _appConfigService = Substitute.For<IAppConfigService>();
        _securityService = new SecurityService(_appConfigService);
        _dbContext = Substitute.For<IUserAccessContext>();
        _command = new CreateCollaboratorCommandHandler(_dbContext, _securityService);
    }
    
    [Fact]
    internal async Task ShouldCreateCollaboratorIntoDatabase()
    {
        // Arrange
        string systemSecurityKeyEnvironmentVariableName = "user_access::DERIVATION_SECURITY_KEY";
        
        _appConfigService.GetEnvironmentVariable(systemSecurityKeyEnvironmentVariableName)
            .Returns("unit-tests-system-security-key");
        
        string secretValidationKeyEnvironmentVariableName = "user_access::JWT_SECURITY_KEY";

        _appConfigService.GetEnvironmentVariable(secretValidationKeyEnvironmentVariableName)
            .Returns("unit-tests-jwt-security-key");

        IQueryable<Collaborator> collaboratorQueryable = new List<Collaborator>().AsQueryable();
        DbSet<Collaborator> mockedCollaboratorDbSet = collaboratorQueryable.BuildMockDbSet();

        _dbContext.Collaborators
            .Returns(mockedCollaboratorDbSet);

        var request = new CreateCollaboratorCommand(
            Guid.NewGuid(),
            ECollaboratorSector.Stock,
            "unittestcollaborator@email.com",
            "secret-unit-test-password");

        var consumerRequest = Substitute.For<ConsumeContext<CreateCollaboratorCommand>>();
        consumerRequest.Message
            .Returns(request);

        Collaborator createdCollaborator = null;
        _dbContext.Collaborators.Add(Arg.Do<Collaborator>(c => createdCollaborator = c));
        
        // Act
        await _command.Consume(consumerRequest);
        
        // Assert
        createdCollaborator
            .Should()
            .NotBeNull();

        createdCollaborator.CollaboratorModuleId
            .Should()
            .Be(request.CollaboratorModuleId);

        createdCollaborator.Sector
            .Should()
            .Be(request.CollaboratorSector);

    }
}