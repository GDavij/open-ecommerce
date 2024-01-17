using Core.Modules.HumanResources.Domain.IntegrationEvents.Dtos;

namespace Core.Modules.HumanResources.Domain.IntegrationEvents.Events.Collaborators;

public record CreatedCollaboratorIntegrationEvent
{
    public CollaboratorDto Collaborator { get; init; }

    public static CreatedCollaboratorIntegrationEvent CreateEvent(CollaboratorDto collaboratorDto)
    {
        return new CreatedCollaboratorIntegrationEvent
        {
            Collaborator = collaboratorDto
        };
    }
};