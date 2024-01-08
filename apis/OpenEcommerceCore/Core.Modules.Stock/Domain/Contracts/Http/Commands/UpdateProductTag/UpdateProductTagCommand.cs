using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProductTag;

public record UpdateProductTagCommand : IRequest<UpdateProductTagCommandResponse>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}