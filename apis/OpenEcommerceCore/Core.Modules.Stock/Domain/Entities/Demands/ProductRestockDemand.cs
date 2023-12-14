using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities.Demands;

internal class ProductRestockDemand
{
    public Guid Id { get; init; }
    public Product Product { get; init; }
    public string Description { get; set; }
    public int RestockQuantity { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdate { get; set; }
    public bool Resolved { get; set; }
    public bool Canceled { get; set; }
    public bool Deleted { get; set; }
    
    // Relationships
    public List<DemandMessage> DemandMessages { get; set; }

    private ProductRestockDemand()
    {}

    public static ProductRestockDemand Create(
        Product product, 
        string description,
        int restockQuantity)
    {
        return new ProductRestockDemand
        {

            Id = Guid.NewGuid(),
            Product = product,
            Description = description,
            RestockQuantity = restockQuantity,
            CreatedAt = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow,
            Resolved = false,
            Canceled = false,
            Deleted = false
        };
    }
}