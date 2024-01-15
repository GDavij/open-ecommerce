namespace Core.Modules.HumanResources.Domain.Entities;

internal sealed class WorkHour
{
    public Guid Id { get; init; }
    public ContributionYear ContributionYear { get; init; }
    public Guid ContributionYearId { get; init; }
    public DateTime Date { get; init; }
    public TimeSpan Start { get; init; }
    public TimeSpan End { get; init; }
    public TimeSpan Duration => End - Start;
}