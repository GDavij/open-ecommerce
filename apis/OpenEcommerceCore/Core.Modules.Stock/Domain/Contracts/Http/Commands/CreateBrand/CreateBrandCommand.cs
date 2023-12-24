using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;

public record CreateBrandCommand
    : IRequest<CreateBrandCommandResponse>
{
    public string Name { get; init; }
    public string? Description { get; init; }
}