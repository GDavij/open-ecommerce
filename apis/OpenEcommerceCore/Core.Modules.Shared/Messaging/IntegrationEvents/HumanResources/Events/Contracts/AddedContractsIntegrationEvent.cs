using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Contracts;

public record AddedContractsIntegrationEvent
{
    public Guid CollaboratorId { get; init; }
    public List<ECollaboratorSector> Sectors { get; init; }
    
    public static AddedContractsIntegrationEvent CreateEvent(Guid collaboratorId, List<ECollaboratorSector> sectors)
    {
        return new AddedContractsIntegrationEvent
        {
            CollaboratorId = collaboratorId,
            Sectors = sectors
        };
    }
};