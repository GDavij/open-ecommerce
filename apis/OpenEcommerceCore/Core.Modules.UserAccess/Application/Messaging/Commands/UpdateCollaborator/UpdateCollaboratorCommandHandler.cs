using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Messaging.Commands.UpdateCollaborator;

internal class UpdateCollaboratorCommandHandler : IUpdateCollaboratorCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;

    public UpdateCollaboratorCommandHandler(IUserAccessContext dbContext, ISecurityService securityService)
    {
        _dbContext = dbContext;
        _securityService = securityService;
    }

    public async Task Consume(ConsumeContext<UpdatedCollaboratorIntegrationEvent> context)
    {
        var updatedCollaboratorInfo = context.Message.CollaboratorUpdated;
        
        var updatedCollaborator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c => c.CollaboratorModuleId == context.Message.CollaboratorUpdated.Id && !c.Deleted);
        
        if (updatedCollaborator is null)
        {
            return;
        }

        updatedCollaborator.Email = updatedCollaboratorInfo.Email;

        if (updatedCollaboratorInfo.Password is not null)
        {
            var newDerivedPassword = await _securityService.DerivePassword(
                updatedCollaboratorInfo.Password,
                updatedCollaborator.SecurityKey,
                default);
                
            updatedCollaborator.Password = newDerivedPassword;
        }
        
        await _dbContext.SaveChangesAsync(default);
    }
}