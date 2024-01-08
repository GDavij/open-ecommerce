using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProductTag;

public record DeleteProductTagCommand : IRequest
{
    public Guid Id { get; init; }
}