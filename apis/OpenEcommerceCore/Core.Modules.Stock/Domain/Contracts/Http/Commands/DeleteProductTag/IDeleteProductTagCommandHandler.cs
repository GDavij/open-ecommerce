using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProductTag;

internal interface IDeleteProductTagCommandHandler
    : IRequestHandler<DeleteProductTagCommand>
{ }