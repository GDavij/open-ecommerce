using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;

public record GetProductQuery : IRequest<GetProductQueryResponse>
{
    public Guid Id { get; init; }
}