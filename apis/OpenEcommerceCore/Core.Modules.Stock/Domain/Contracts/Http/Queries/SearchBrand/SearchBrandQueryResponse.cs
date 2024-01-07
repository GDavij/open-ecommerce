namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;



public record SearchBrandQueryResponse
{
    public List<FoundedBrandSearch> BrandsFound { get; init; }
    public int MaxPages { get; init; }
    public int pageIndex { get; init; } = 1;
    
    
    public record FoundedBrandSearch
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
};
