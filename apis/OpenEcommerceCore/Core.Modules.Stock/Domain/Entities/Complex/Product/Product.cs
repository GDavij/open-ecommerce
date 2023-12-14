using Core.Modules.Stock.Domain.Entities.Complex.Product.ProductDetails;
using Core.Modules.Stock.Domain.Entities.Demands;

namespace Core.Modules.Stock.Domain.Entities.Complex.Product;
/*
 * If Possible Find a better alternative to declare it's Sub Types that not add much complexity to the main class and files in the project
 * the actual method is okay, but not good enough (Much Files on entities)
 */
internal class Product
{
    public Guid Id { get; init; }
    public Brand Brand { get; init; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public string EAN { get; set; }
    public string? UPC { get; set; }
    public decimal Price { get; set; }
    public int StockUnitCount { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdate { get; set; }

    // Relationships
    public List<Supplier> Suppliers { get; set; }
    public List<ProductTag> Tags { get; set; }
    public List<ProductImage> Images { get; set; }
    public List<MeasurementDetail> Measurements { get; set; }
    public List<TechnicalDetail> TechnicalDetails { get; set; }
    public List<OtherDetail> OtherDetails { get; set; }
    public List<ProductRestockDemand> ProductRestockDemands { get; set; }
    
    private Product()
    {}

    public static Product Create(
        Brand brand,
        string name,
        string description,
        string? sku,
        string ean,
        string? upc,
        decimal price,
        int stockUnitCount) {
        return new Product{
            Id = Guid.NewGuid(),
            Brand =brand,
            Name = name,
            Description = description,
            SKU = sku,
            EAN = ean,
            UPC = upc,
            Price = price,
            StockUnitCount = stockUnitCount,
            CreatedAt = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow
        };
    }
}

