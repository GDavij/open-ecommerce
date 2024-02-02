using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.SearchContracts;

internal interface ISearchContractsQueryHandler
    : IRequestHandler<SearchContractsQuery, PaginatedList<SearchContractsQueryResponse>>
{ }