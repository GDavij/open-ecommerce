using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetMeasureUnit;

internal interface IGetMeasureUnitQueryHandler
    : IRequestHandler<GetMeasureUnitQuery, GetMeasureUnitQueryResponse>
{ }