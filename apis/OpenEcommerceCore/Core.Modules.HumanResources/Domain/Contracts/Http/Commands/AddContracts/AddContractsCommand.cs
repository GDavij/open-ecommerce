using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedSchemas;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.AddContracts;

public record AddContractsCommand : IRequest<AddContractsCommandResponse>
{
    public Guid CollaboratorId { get; init; }
    public List<ContractRequestSchema> Contracts { get; init; }
};