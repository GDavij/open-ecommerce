using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Messaging.Commands.DeleteCollaborator;

internal class DeleteCollaboratorCommandHandler : IDeleteCollaboratorCommandHandler
{
    private readonly IUserAccessContext _dbContext;

    public DeleteCollaboratorCommandHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<DeletedCollaboratorIntegrationEvent> context)
    {
        var collaboratorId = context.Message.Id;
        var existentCollaborator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c => c.CollaboratorModuleId == collaboratorId);
        
        if (existentCollaborator is null)
        {
            return;
        }
        
        existentCollaborator.Deleted = true;
        await _dbContext.SaveChangesAsync();
    }
}