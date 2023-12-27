using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;

public interface IUpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
{ }