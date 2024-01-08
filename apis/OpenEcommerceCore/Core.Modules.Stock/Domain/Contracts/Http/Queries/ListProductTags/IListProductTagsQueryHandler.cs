using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.ListProductTags;

internal interface IListProductTagsQueryHandler
    : IRequestHandler<ListProductTagsQuery, PaginatedList<ListProductTagsQueryResponse>>
{ }