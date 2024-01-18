using Core.Modules.HumanResources.Domain.IntegrationEvents.Dtos;

namespace Core.Modules.HumanResources.Domain.IntegrationEvents.Events.Collaborators;

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