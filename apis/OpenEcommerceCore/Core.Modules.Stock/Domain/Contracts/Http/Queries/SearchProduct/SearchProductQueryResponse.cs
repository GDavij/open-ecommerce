namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchProduct;

public record SearchProductQueryResponse
{
    public List<FoundedProductSearch> ProductsFound { get; init; }
    public int MaxPages { get; init; }
    public int pageIndex { get; init; } = 1;
};

public record FoundedProductSearch
{
    public Guid ProductId { get; init; }
    public string BrandName { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public string? Sku { get; init; }
    public string Ean { get; init; }
    public string? Upc { get; init; }
    public decimal Price { get; init; }
    public List<ProductTagResponse> Tags { get; init; }
    public int StockUnitCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdate { get; init; }
}


public record ProductTagResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}