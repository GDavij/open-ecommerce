using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.AddContracts;

internal interface IAddContractsCommandHandler
    : IRequestHandler<AddContractsCommand, AddContractsCommandResponse>
{ }