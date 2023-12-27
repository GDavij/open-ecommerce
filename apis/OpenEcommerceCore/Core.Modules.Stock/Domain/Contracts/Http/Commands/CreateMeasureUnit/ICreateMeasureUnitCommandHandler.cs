using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;

public interface ICreateMeasureUnitCommandHandler
    : IRequestHandler<CreateMeasureUnitCommand, CreateMeasureUnitCommandResponse>
{ }