using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchProduct;

public interface ISearchProductQueryHandler
    : IRequestHandler<SearchProductQuery, PaginatedList<SearchProductQueryResponse>>
{ }