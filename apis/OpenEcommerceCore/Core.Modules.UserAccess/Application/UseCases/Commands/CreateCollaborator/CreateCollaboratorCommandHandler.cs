using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;
using Core.Modules.HumanResources.Domain.IntegrationEvents.Events.Collaborators;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaborator;

internal class CreateCollaboratorCommandHandler : ICreateCollaboratorCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;

    public CreateCollaboratorCommandHandler(IUserAccessContext dbContext, ISecurityService securityService)
    {
        _dbContext = dbContext;
        _securityService = securityService;
    }

    public async Task Consume(ConsumeContext<CreatedCollaboratorIntegrationEvent> context)
    {
        // Implement Retry and verification for databaseConnection Health from start
        var createdCollaborator = context.Message.CollaboratorCreated;
        var existentCollaborator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c =>
                c.Email == createdCollaborator.Email &&
                c.Deleted == false);

        if (existentCollaborator != null)
        {
            // Add More Error Safe Error handling
            return;
        }

        byte[] securityKey = _securityService.GenerateSecurityKey();
        byte[] derivedPassword = await _securityService.DerivePassword(
            createdCollaborator.Password,
            securityKey,
            default);

        var collaborator = Collaborator.Create(
            Guid.NewGuid(), 
            createdCollaborator.Id,
            createdCollaborator.Email,
            derivedPassword,
            securityKey,
            createdCollaborator.Sectors);

        _dbContext.Collaborators.Add(collaborator);

        await _dbContext.SaveChangesAsync();
    }
}