using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteContract;

internal interface IDeleteContractCommandHandler
    : IRequestHandler<DeleteContractCommand>
{ }