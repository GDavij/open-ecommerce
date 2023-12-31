using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProduct;

public record DeleteProductCommand : IRequest
{
    public Guid Id { get; init; }
};