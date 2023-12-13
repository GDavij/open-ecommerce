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
    public List<Supplier> Suppliers { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<ProductTag> Tags { get; set; }
    public List<ProductImage> Images { get; set; }
    public string? SKU { get; set; }
    public string EAN { get; set; }
    public string? UPC { get; set; }
    public decimal Price { get; set; }
    public int StockUnitCount { get; set; }
    public List<ProductDetail> Measurements { get; set; }
    public List<ProductDetail> TechnicalDetails { get; set; }
    public List<ProductDetail> OtherDetails { get; set; }
    public List<ProductRestockDemand> ProductRestockDemands { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdate { get; set; }

    private Product(
        Guid id,
        Brand brand,
        List<Supplier> suppliers,
        string name,
        string description,
        List<ProductTag> tags,
        List<ProductImage> images,
        string sku,
        string ean,
        string upc,
        decimal price,
        int stockUnitCount,
        List<ProductDetail> measurements,
        List<ProductDetail> technicalDetails,
        List<ProductDetail> otherDetails,
        List<ProductRestockDemand> productRestockDemands,
        DateTime createdAt,
        DateTime lastUpdate)
    {
        Id = id;
        Brand = brand;
        Suppliers = suppliers;
        Name = name;
        Description = description;
        Tags = tags;
        Images = images;
        SKU = sku;
        EAN = ean;
        UPC = upc;
        Price = price;
        StockUnitCount = stockUnitCount;
        Measurements = measurements;
        TechnicalDetails = technicalDetails;
        OtherDetails = otherDetails;
        ProductRestockDemands = productRestockDemands;
        CreatedAt = createdAt;
        LastUpdate = lastUpdate;
    }

    public Product Create(
        Guid id,
        Brand brand,
        List<Supplier> suppliers,
        string name,
        string description,
        List<ProductTag> tags,
        string sku,
        string ean,
        string upc,
        decimal price,
        int stockUnitCount,
        List<ProductDetail> measurements,
        List<ProductDetail> technicalDetails,
        List<ProductDetail> otherDetails) {
        var createdProduct = new Product(
            id,
            brand,
            suppliers,
            name,
            description,
            tags,
            new List<ProductImage>(),
            sku,
            ean,
            upc,
            price,
            stockUnitCount,
            measurements,
            technicalDetails,
            otherDetails,
            new List<ProductRestockDemand>(),
            DateTime.UtcNow,
            DateTime.UtcNow);

        return createdProduct;
    }
}

