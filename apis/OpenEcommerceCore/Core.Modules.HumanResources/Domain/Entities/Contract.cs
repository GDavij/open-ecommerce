using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Entities;

internal sealed class Contract
{
    public Guid Id { get; init; }
    public Collaborator Collaborator { get; init; }
    public Guid CollaboratorId { get; init; }
    public string Name { get; init; }
    public ECollaboratorSector Sector { get; init; }
    public List<ContributionYear> ContributionYears { get; init; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set;}
    public decimal MonthlySalary { get; set; }
    public bool Expired => DateTime.UtcNow > EndDate;
    public bool Broken { get; set; }
    public bool Deleted { get; set; }
}
