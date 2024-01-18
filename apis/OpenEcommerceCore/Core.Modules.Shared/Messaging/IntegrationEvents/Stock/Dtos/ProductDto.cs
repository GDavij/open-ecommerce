namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

public record ProductDto
{
    public Guid Id { get; init; }
    public BrandDto Brand { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public int StockUnitCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdate { get; init; }

    public List<ProductTagDto> Tags { get; init; }
    public List<ProductImageDto> Images { get; init; }
    public List<ProductDetailDto> Measurements { get; init; }
    public List<ProductDetailDto> TechnicalDetails { get; init; }
    public List<ProductDetailDto> OtherDetails { get; init; }

    private ProductDto()
    {}

    public static ProductDto Create(
        Guid id,
        BrandDto brand,
        string name,
        string? description,
        decimal price,
        int stockUnitCount,
        DateTime createdAt,
        DateTime lastUpdate,
        List<ProductTagDto> tags,
        List<ProductImageDto> images,
        List<ProductDetailDto> measurements,
        List<ProductDetailDto> technicalDetails,
        List<ProductDetailDto> otherDetails)
    {
        return new ProductDto
        {
            Id = id,
            Brand = brand,
            Name = name,
            Description = description,
            Price = price,
            StockUnitCount = stockUnitCount,
            CreatedAt = createdAt,
            LastUpdate = lastUpdate,
            Tags = tags,
            Images = images,
            Measurements = measurements,
            TechnicalDetails = technicalDetails,
            OtherDetails = otherDetails
        };
    }
}