namespace Core.Modules.HumanResources.Domain.Entities;
internal sealed class ContributionYear
{
    public Guid Id { get; init; }
    public Contract Contract { get; init; }
    public Guid ContractId { get; init; }
    public int Year { get; set; }
    public List<WorkHour> WorkHours { get; init; }
}
