using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.ListMeasureUnits;

internal interface IListMeasureUnitsQueryHandler
    : IRequestHandler<ListMeasureUnitsQuery, PaginatedList<ListMeasureUnitsQueryResponse>>
{ }