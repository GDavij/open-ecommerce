using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities;

internal class ProductRestockOrder
{
    public Guid Id { get; init; }
    public Product Product { get; init; }
    public Distributor Distributor { get; set; }
    public string Description { get; set; }
    public List<TradeMessage> TradeMessages { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdate { get; set; }
    public bool Resolved { get; set; }
    public bool Deleted { get; set; }

    private ProductRestockOrder(
        Guid id,
        Product product,
        Distributor distributor,
        string description,
        List<TradeMessage> tradeMessages,
        DateTime createdAt,
        DateTime lastUpdate,
        bool resolved,
        bool deleted)
    {
        Id = id;
        Product = product;
        Distributor = distributor;
        Description = description;
        TradeMessages = tradeMessages;
        CreatedAt = createdAt;
        LastUpdate = lastUpdate;
        Resolved = resolved;
        Deleted = deleted;
        Description = description;
    }

    public ProductRestockOrder Create(Guid id, Product product, Distributor distributor, string description)
    {
        return new ProductRestockOrder(
            id,
            product,
            distributor,
            description,
            new List<TradeMessage>(),
            DateTime.UtcNow,
            DateTime.UtcNow,
            false,
            false);
    }
}

internal class TradeMessage
{
    public Guid Id { get; set; }
    public ProductRestockOrder ProductRestockOrder { get; set; }
    public Collaborator Collaborator { get; set; }
    public ECollaboratorSector Sector { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Deleted { get; set; }

    private TradeMessage(
        Guid id,
        ProductRestockOrder productRestockOrder,
        Collaborator collaborator,
        ECollaboratorSector sector,
        string message,
        DateTime createdAt,
        bool deleted)
    {
        Id = id;
        ProductRestockOrder = productRestockOrder;
        Collaborator = collaborator;
        Sector = sector;
        Message = message;
        CreatedAt = createdAt;
        Deleted = deleted;
    }

    public TradeMessage Create(
        Guid id,
        ProductRestockOrder productRestockOrder,
        Collaborator collaborator,
        ECollaboratorSector sector,
        string message,
        DateTime createdAt,
        bool deleted)
    {
        return new TradeMessage(
            id,
            productRestockOrder,
            collaborator,
            sector,
            message,
            createdAt,
            deleted);
    }
}