using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteMeasureUnit;

public record DeleteMeasureUnitCommand : IRequest
{
    public Guid Id { get; init; }
}