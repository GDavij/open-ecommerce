using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.ListProductTags;

public record ListProductTagsQuery : IRequest<PaginatedList<ListProductTagsQueryResponse>>
{
    public int Page { get; init; }
}