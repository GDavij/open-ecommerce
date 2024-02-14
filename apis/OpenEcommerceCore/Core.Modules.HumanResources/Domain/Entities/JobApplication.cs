using Core.Modules.HumanResources.Domain.Enums;
using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Entities;

internal sealed class JobApplication
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public int Age { get; init; }
    public ECollaboratorSector Sector { get; init; }
    public ApplicationProcess ProcessStep { get; set; }
    public string ResumeURL { get; init; }
    public List<SocialLink> SocialLinks { get; init; }
    public DateTime CreatedAt { get; init; }
}


