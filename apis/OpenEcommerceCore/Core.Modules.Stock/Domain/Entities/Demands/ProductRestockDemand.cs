using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities.Demands;

internal class ProductRestockDemand
{
    public Guid Id { get; init; }
    public Product Product { get; init; }
    public string Description { get; set; }
    public int RestockQuantity { get; set; }
    public List<DemandMessage> DemandMessages { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdate { get; set; }
    public bool Resolved { get; set; }
    public bool Canceled { get; set; }
    public bool Deleted { get; set; }

    private ProductRestockDemand(
        Guid id,
        Product product,
        string description,
        int restockQuantity,
        List<DemandMessage> demandMessages,
        DateTime createdAt,
        DateTime lastUpdate,
        bool resolved,
        bool canceled,
        bool deleted)
    {
        Id = id;
        Product = product;
        Description = description;
        RestockQuantity = restockQuantity;
        DemandMessages = demandMessages;
        CreatedAt = createdAt;
        LastUpdate = lastUpdate;
        Resolved = resolved;
        Canceled = canceled;
        Deleted = deleted;
        Description = description;
    }

    public ProductRestockDemand Create(Guid id, Product product, string description, int restockQuantity = 1)
    {
        return new ProductRestockDemand(
            id,
            product,
            description,
            restockQuantity,
            new List<DemandMessage>(),
            DateTime.UtcNow,
            DateTime.UtcNow,
            false,
            false,
            false);
    }
}