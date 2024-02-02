using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddContracts;

internal interface IAddContractsCommandHandler
    : IRequestHandler<AddContractsCommand, AddContractsCommandResponse>
{ }