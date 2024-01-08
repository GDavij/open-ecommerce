namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.ListProductTags;

public record ListProductTagsQueryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}