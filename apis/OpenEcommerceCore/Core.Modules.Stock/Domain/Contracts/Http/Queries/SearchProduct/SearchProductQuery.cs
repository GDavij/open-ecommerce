using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchProduct;

public record SearchProductQuery : IRequest<PaginatedList<SearchProductQueryResponse>>
{
    public int Page { get; init; }
    public string? SearchTerm { get; init; }
    public List<Guid> BrandsIds { get; init; } = new List<Guid>();
    public List<Guid> TagsIds { get; init; } = new List<Guid>();
};