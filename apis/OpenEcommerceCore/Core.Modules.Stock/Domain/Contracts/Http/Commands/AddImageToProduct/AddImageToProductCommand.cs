using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;

public record AddImageToProductCommand 
    : IRequest<AddImageToProductCommandResponse>
{
    public Guid ProductId { get; init; }
    public string Description { get; init; }
    public IFormFile ImageFile { get; init; }
};