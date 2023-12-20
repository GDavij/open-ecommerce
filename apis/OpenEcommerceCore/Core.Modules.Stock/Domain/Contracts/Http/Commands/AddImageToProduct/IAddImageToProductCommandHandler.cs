using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;

public interface IAddImageToProductCommandHandler 
    : IRequestHandler<AddImageToProductCommand, AddImageToProductCommandResponse>
{ }