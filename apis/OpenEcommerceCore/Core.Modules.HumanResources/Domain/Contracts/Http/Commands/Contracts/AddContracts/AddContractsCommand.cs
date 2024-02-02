using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedSchemas;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddContracts;

public record AddContractsCommand : IRequest<AddContractsCommandResponse>
{
    public Guid CollaboratorId { get; init; }
    public List<ContractRequestSchema> Contracts { get; init; }
};