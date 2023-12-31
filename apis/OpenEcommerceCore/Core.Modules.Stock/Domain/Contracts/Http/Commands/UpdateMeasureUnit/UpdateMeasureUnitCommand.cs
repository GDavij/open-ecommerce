using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateMeasureUnit;

public record UpdateMeasureUnitCommand : IRequest<UpdateMeasureUnitCommandResponse>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? ShortName { get; init; }
    public string Symbol { get; set; }
};