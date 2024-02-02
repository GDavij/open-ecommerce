using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.DeleteContract;

internal interface IDeleteContractCommandHandler
    : IRequestHandler<DeleteContractCommand>
{ }