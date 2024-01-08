using Core.Modules.Shared.Domain.DataStructures;
using Core.Modules.Shared.Domain.Extensions;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.ListMeasureUnits;

namespace Core.Modules.Stock.Application.Http.Queries.ListMeasureUnits;

internal class ListMeasureUnitsQueryHandler : IListMeasureUnitsQueryHandler
{
    private const int _rowsPerPage = 50;
    private readonly IStockContext _dbContext;

    public ListMeasureUnitsQueryHandler(IStockContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<ListMeasureUnitsQueryResponse>> Handle(ListMeasureUnitsQuery request, CancellationToken cancellationToken)
    {
        var measureUnits = await _dbContext.MeasureUnits
            .Select(m => new ListMeasureUnitsQueryResponse
            {
                Id = m.Id,
                Name = m.Name,
                Symbol = m.Symbol
            })
            .ToPaginatedListAsync(_rowsPerPage, request.Page, cancellationToken);

        return measureUnits;
    }
}