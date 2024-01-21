using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;

public record UpdatedCollaboratorIntegrationEvent
{
    public CollaboratorUpdatedDto CollaboratorUpdated { get; init; }
    
    public static UpdatedCollaboratorIntegrationEvent CreateEvent(CollaboratorUpdatedDto collaboratorUpdatedDto)
    {
        return new UpdatedCollaboratorIntegrationEvent
        {
            CollaboratorUpdated = collaboratorUpdatedDto
        };
    }
};