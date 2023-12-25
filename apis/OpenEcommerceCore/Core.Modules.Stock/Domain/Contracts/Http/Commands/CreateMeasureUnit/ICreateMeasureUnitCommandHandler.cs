using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;

internal interface ICreateMeasureUnitCommandHandler
    : IRequestHandler<CreateMeasureUnitCommand, CreateMeasureUnitCommandResponse>
{ }