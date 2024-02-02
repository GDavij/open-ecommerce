using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.BreakContract;

internal interface IBreakContractCommandHandler
    : IRequestHandler<BreakContractCommand>
{ }