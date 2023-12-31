using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateMeasureUnit;

public interface IUpdateMeasureUnitCommandHandler
    : IRequestHandler<UpdateMeasureUnitCommand, UpdateMeasureUnitCommandResponse>
{ }