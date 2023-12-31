using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;

public record UpdateBrandCommand : IRequest<UpdateBrandCommandResponse>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}