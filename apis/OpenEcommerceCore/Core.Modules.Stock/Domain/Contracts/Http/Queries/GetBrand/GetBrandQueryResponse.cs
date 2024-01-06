namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetBrand;

public record GetBrandQueryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public List<ProductResponse> Products { get; init; }

    public record ProductResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public int StockUnitCount { get; init; }
        public List<TagResponse> Tags { get; init; }
    }

    public record TagResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }
};
