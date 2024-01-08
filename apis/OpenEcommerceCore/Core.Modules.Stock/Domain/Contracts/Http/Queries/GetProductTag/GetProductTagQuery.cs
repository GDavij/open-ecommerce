using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProductTag;

public record GetProductTagQuery : IRequest<GetProductTagQueryResponse>
{
    public Guid Id { get; init; }
}