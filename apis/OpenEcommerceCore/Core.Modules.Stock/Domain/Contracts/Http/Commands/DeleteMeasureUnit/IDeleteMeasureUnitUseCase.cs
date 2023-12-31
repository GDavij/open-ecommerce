using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteMeasureUnit;

public interface IDeleteMeasureUnitUseCase
    : IRequestHandler<DeleteMeasureUnitCommand>
{ }