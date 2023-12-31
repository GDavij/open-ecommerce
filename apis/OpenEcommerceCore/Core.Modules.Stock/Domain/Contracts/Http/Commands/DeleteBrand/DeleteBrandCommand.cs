using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteBrand;

public record DeleteBrandCommand : IRequest
{
    public Guid Id { get; init; }
}