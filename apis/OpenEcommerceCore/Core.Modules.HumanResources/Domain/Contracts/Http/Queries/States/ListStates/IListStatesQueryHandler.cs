using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.States.ListStates;

internal interface IListStatesQueryHandler
    : IRequestHandler<ListStatesQuery, PaginatedList<ListStatesQueryResponse>>
{ }