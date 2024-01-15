namespace Core.Modules.HumanResources.Domain.Entities;

internal sealed class Collaborator
{
    public Guid Id { get; init; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    public int Age { get; init; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public List<Contract> Contracts { get; init; }
    public List<SocialLink> SocialLinks { get; init; }
    public List<Address> Addresses { get; init; }
    public bool Deleted { get; set; }
    public int TotalContributionYears => Contracts.Sum(c => c.ContributionYears.Count);
    public double TotalHoursWorked => Contracts.Sum(c => c.ContributionYears.Sum(cy => cy.WorkHours.Sum(wh => wh.Duration.Ticks)));
}