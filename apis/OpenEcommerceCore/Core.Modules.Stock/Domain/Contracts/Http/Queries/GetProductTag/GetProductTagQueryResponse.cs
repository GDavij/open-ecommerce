namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProductTag;

public record GetProductTagQueryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}