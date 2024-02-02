using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.SearchContracts;

public record SearchContractsQueryResponse
{
    public Guid Id { get; init; }
    public string CollaboratorName { get; init; }
    public Guid CollaboratorId { get; init; }
    public string Name { get; init; }
    public ECollaboratorSector Sector { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public bool Expired { get; init; }
    public bool Broken { get; init; }
    public bool Deleted { get; init; }
}