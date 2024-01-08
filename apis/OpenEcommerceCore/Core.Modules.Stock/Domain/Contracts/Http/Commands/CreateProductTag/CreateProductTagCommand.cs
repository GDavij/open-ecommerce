using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProductTag;

public record CreateProductTagCommand : IRequest<CreateProductTagCommandResponse>
{
    public string Name { get; init; }
}