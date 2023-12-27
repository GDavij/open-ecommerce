using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;

public record RemoveImageFromProductCommand : IRequest<RemoveImageFromProductCommandResponse>
{
    public Guid Id { get; init; }
}