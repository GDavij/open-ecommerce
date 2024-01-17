using Core.Modules.HumanResources.Domain.Enums;

namespace Core.Modules.HumanResources.Domain.Entities;

internal sealed class SocialLink
{
    public Guid Id { get; init; }
    public Collaborator? Collaborator { get; init; }
    public Guid? CollaboratorId { get; init; }
    public JobApplication? JobApplication { get; init; }
    public Guid? JobApplicationId { get; init; }
    public SocialMedia SocialMedia { get; init; }
    public string URL { get; init; }
}

