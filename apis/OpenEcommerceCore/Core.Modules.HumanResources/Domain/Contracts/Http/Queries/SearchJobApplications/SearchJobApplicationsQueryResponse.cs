using Core.Modules.HumanResources.Domain.Enums;
using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.SearchJobApplication;

public record SearchJobApplicationsQueryResponse
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public int Age { get; init; }
    public ECollaboratorSector Sector { get; init; }
    public ApplicationProcess ProcessStep { get; init; }
    public DateTime SentAt { get; init; }
};