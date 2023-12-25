using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;

public record CreateMeasureUnitCommand
    : IRequest<CreateMeasureUnitCommandResponse>
{
    public string Name { get; init; }
    public string? ShortName { get; init; }
    public string Symbol { get; init; }
}