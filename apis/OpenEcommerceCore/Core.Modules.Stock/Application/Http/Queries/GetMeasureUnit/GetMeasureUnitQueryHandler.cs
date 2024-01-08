using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetMeasureUnit;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Queries.GetMeasureUnit;

internal class GetMeasureUnitQueryHandler : IGetMeasureUnitQueryHandler
{
    private readonly IStockContext _dbContext;

    public GetMeasureUnitQueryHandler(IStockContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetMeasureUnitQueryResponse> Handle(GetMeasureUnitQuery request, CancellationToken cancellationToken)
    {
        var measureUnit = await _dbContext.MeasureUnits
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (measureUnit is null)
        {
            throw new InvalidMeasureUnitException(request.Id);
        }

        return new GetMeasureUnitQueryResponse
        {
            Id = measureUnit.Id,
            Name = measureUnit.Name,
            ShortName = measureUnit.ShortName,
            Symbol = measureUnit.Symbol
        };
    }
}