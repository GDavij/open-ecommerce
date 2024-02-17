using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.States.ListStates;

public record ListStatesQuery(int Page = 1) : IRequest<PaginatedList<ListStatesQueryResponse>>;