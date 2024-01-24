using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.BreakContract;

internal interface IBreakContractCommandHandler
    : IRequestHandler<BreakContractCommand>
{ }