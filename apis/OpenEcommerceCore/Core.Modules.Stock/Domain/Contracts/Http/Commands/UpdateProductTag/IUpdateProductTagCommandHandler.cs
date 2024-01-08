using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProductTag;

internal interface IUpdateProductTagCommandHandler
    : IRequestHandler<UpdateProductTagCommand, UpdateProductTagCommandResponse>
{ }