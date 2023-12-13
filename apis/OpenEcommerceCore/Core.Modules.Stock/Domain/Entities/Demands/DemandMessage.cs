using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Stock.Domain.Entities.Demands;

namespace Core.Modules.Stock.Domain.Entities;

internal class DemandMessage
{
    public Guid Id { get; set; }
    public ProductRestockDemand ProductRestockDemand { get; set; }
    public Collaborator Collaborator { get; set; }
    public ECollaboratorSector Sector { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Deleted { get; set; }

    private DemandMessage(
        Guid id,
        ProductRestockDemand productRestockDemand,
        Collaborator collaborator,
        ECollaboratorSector sector,
        string message,
        DateTime createdAt,
        bool deleted)
    {
        Id = id;
        ProductRestockDemand = productRestockDemand;
        Collaborator = collaborator;
        Sector = sector;
        Message = message;
        CreatedAt = createdAt;
        Deleted = deleted;
    }

    public DemandMessage Create(
        Guid id,
        ProductRestockDemand productRestockDemand,
        Collaborator collaborator,
        ECollaboratorSector sector,
        string message,
        DateTime createdAt,
        bool deleted)
    {
        return new DemandMessage(
            id,
            productRestockDemand,
            collaborator,
            sector,
            message,
            createdAt,
            deleted);
    }
}