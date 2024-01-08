using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.ListMeasureUnits;

public record ListMeasureUnitsQuery : IRequest<PaginatedList<ListMeasureUnitsQueryResponse>>
{
    public int Page { get; init; }
}