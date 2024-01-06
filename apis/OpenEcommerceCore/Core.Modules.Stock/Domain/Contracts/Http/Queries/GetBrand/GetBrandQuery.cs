using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetBrand;

public record GetBrandQuery : IRequest<GetBrandQueryResponse>
{
    public Guid Id { get; init; }
};