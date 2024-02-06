using Core.Modules.HumanResources.Domain.Enums;
using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.GetJobApplication;

public record GetJobApplicationQueryResponse
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public int Age { get; init; }
    public ECollaboratorSector Sector { get; init; }
    public ApplicationProcess ProcessStep { get; init; }
    public string Resume { get; init; }
    public List<SocialLink> SocialLinks { get; init; }
    public DateTime SentAt { get; init; }
    
    public record SocialLink
    {
        public Guid Id { get; init; }
        public SocialMedia SocialMedia { get; init; }
        public string URL { get; init; }
    }
}