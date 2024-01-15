namespace Core.Modules.HumanResources.Domain.Entities;

internal sealed class JobApplication
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public int Age { get; init; }
    public SectorToWork Sector { get; init; }
    public ApplicationProcess ProcessStep { get; init; }
    public string ResumeURL { get; init; }
    public List<SocialLink> SocialLinks { get; init; }
}

internal enum SectorToWork
{
    Stock,
    HumanResources
}

internal enum ApplicationProcess
{
    OnDatabase,
    Analysis,
    Interview,
    Hired
}
