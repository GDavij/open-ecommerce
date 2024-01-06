using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;

internal interface ISearchBrandQueryHandler
    : IRequestHandler<SearchBrandQuery, SearchBrandQueryResponse>
{ }