using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Stock.Domain.Entities.Demands;

namespace Core.Modules.Stock.Domain.Entities;

internal class DemandMessage
{
    public Guid Id { get; init; }
    public ProductRestockDemand ProductRestockDemand { get; init; }
    public Collaborator Collaborator { get; init; }
    public ECollaboratorSector Sector { get; init; }
    public string Message { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool Deleted { get; set; }

    private DemandMessage()
    {}

    public static DemandMessage Create(
        ProductRestockDemand productRestockDemand,
        Collaborator collaborator,
        ECollaboratorSector sector,
        string message,
        DateTime createdAt,
        bool deleted)
    {
        return new DemandMessage
        {
           Id = Guid.NewGuid(),
           ProductRestockDemand = productRestockDemand,
           Collaborator = collaborator,
           Sector = sector,
           Message = message,
           CreatedAt = createdAt,
           Deleted = deleted
        };
    }
}