using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetMeasureUnit;

public record GetMeasureUnitQuery : IRequest<GetMeasureUnitQueryResponse>
{
    public Guid Id { get; init; }
}