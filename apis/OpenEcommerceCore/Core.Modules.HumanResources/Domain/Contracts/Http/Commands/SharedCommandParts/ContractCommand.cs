using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedCommandParts;


public record ContractCommand
{
    public string Name { get; init; }
    public ECollaboratorSector Sector { get; init; }
    public List<ContributionYears> ContributionsYears { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public decimal MonthlySalary { get; init; }
    public bool Broken { get; init; }

    public record ContributionYears
    {
        public int Year { get; init; }
        public List<WorkHour> WorkHours { get; init; }

        public record WorkHour
        {
            public DateTime Date { get; init; }
            public TimeSpan Start { get; init; }
            public TimeSpan End { get; init; }
        }
    }
}