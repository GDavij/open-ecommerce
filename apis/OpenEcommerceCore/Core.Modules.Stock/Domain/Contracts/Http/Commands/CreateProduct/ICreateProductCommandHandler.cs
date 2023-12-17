using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;

public interface ICreateProductCommandHandler
    : IRequestHandler<CreateProductCommand>
{ }