using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;

internal class RemoveImageFromProductCommand : IRequest<RemoveImageFromProductCommandResponse>
{
    public Guid Id { get; init; }
}