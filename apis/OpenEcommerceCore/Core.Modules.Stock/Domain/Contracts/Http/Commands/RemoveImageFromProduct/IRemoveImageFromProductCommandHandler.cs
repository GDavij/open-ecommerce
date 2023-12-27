using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;

public interface IRemoveImageFromProductCommandHandler : IRequestHandler<RemoveImageFromProductCommand, RemoveImageFromProductCommandResponse>
{ }