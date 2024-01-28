using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Contracts;

public record BrokeContractIntegrationEvent
{
    public Guid CollaboratorId { get; init; }
    public ECollaboratorSector Sector { get; init; }

    public static BrokeContractIntegrationEvent CreateEvent(Guid collaboratorId, ECollaboratorSector sector)
    {
        return new BrokeContractIntegrationEvent
        {
            CollaboratorId = collaboratorId,
            Sector = sector
        };
    }
};