using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.GetContract;

public record GetContractQueryResponse
{
    public Guid Id { get; init; }
    public Guid CollaboratorId { get; init; }
    public string Name { get; init; }
    public  ECollaboratorSector Sector { get; init; }
    public List<ContributionYear> ContributionYears { get; init; }
    public List<SuggestedCollaboratorContracts> SimilarCollaboratorContracts { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public decimal MonthlySalary { get; init; }
    public bool Expired { get; init; }
    public bool Broken { get; init; }
    public bool Deleted { get; init; } 

    public record SuggestedCollaboratorContracts
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public ECollaboratorSector Sector { get; init; }
    }
    
    public record ContributionYear
    {
        public Guid Id { get; init; }
        public int Year { get; init; }
        public List<WorkHour> WorkHours { get; init; }

        public record WorkHour
        {
            public Guid Id { get; init; }
            public DateTime Date { get; init; }
            public TimeSpan Start { get; init; }
            public TimeSpan End { get; init; }
            public TimeSpan Duration { get; init; }
        }
    }
}