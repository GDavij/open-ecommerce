using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Domain.Enums;
using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries;

public record GetCollaboratorQueryResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Description { get; init; }
    public int Age { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public List<ContractResponse> Contracts { get; init; }
    public List<SocialLinkResponse> SocialLinks { get; init; }
    public List<AddressResponse> Addresses { get; init; }
    public int TotalContributionYears { get; init; }
    public double TotalHoursWorked { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsDeleted { get; init; }
    
    public record AddressResponse
    {
        public Guid Id { get; init; }
        public Guid CollaboratorId { get; init; }
        public StateResponse State { get; init; }
        public int ZipCode { get; init; }
        public string Neighbourhood { get; init; }
        public string Street { get; init; }
        
        public record StateResponse {
            public Guid Id { get; init; }
            public string Name { get; init; }
            public string ShortName { get; init; }
        }
    }
    
    public record SocialLinkResponse
    {
        public Guid Id { get; init; }
        public Guid CollaboratorId { get; init; }
        public SocialMedia SocialMedia { get; init; }
        public string URL { get; init; }
    }
    
    public record ContractResponse
    {
        public Guid Id { get; init; }
        public Guid CollaboratorId { get; init; }
        public string Name { get; init; }
        public ECollaboratorSector CollaboratorSector { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public bool IsInForce { get; init; }
        public decimal MonthlySalary { get; init; }
        public bool Expired { get; init; }
        public bool Broken { get; init; }
        public bool Deleted { get; init; }
    }
}