namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;

public record GetProductQueryResponse
{
    public Guid Id { get; init; }
    public Guid BrandId { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public string? Sku { get; init; }
    public string Ean { get; init; }
    public string? Upc { get; init; }
    public decimal Price { get; init; }
    public int StockUnitCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastUpdate { get; init; }
    public List<Guid> ProductSuppliers { get; init; }
    public List<TagResponse> ProductTags { get; init; }
    public List<ImageResponse> ProductImages { get; init; }
    public List<ProductDetailResponse> Measurements { get; init; }
    public List<ProductDetailResponse> TechnicalDetails { get; init; }
    public List<ProductDetailResponse> OtherDetails { get; init; }

    public GetProductQueryResponse()
    { }


    public record ImageResponse
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public string Url { get; init; }
    }

    public record TagResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }

    public record ProductDetailResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Value { get; init; }
        public int ShowOrder { get; init; }
        public MeasureUnitResponse? MeasureUnit { get; init; }
    }

    public record MeasureUnitResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string? ShortName { get; init; }
        public string Symbol { get; init; }
    }
}
