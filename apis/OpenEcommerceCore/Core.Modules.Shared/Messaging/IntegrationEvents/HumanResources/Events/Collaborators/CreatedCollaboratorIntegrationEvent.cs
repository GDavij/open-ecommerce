using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;

public record CreatedCollaboratorIntegrationEvent
{
    public CollaboratorCreatedDto CollaboratorCreated { get; init; }

    public static CreatedCollaboratorIntegrationEvent CreateEvent(CollaboratorCreatedDto collaboratorCreatedDto)
    {
        return new CreatedCollaboratorIntegrationEvent
        {
            CollaboratorCreated = collaboratorCreatedDto
        };
    }
};