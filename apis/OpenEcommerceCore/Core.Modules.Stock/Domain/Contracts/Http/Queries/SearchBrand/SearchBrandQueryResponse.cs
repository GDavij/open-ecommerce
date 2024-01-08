namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;



public record SearchBrandQueryResponse
{
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
};
