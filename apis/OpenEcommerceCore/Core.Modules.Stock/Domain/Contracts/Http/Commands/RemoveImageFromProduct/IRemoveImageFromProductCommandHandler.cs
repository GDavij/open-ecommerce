using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;

internal interface IRemoveImageFromProductCommandHandler : IRequestHandler<RemoveImageFromProductCommand, RemoveImageFromProductCommandResponse>
{ }