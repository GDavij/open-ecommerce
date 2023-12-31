using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProduct;

internal interface IDeleteProductCommandHandler 
    : IRequestHandler<DeleteProductCommand>
{ }