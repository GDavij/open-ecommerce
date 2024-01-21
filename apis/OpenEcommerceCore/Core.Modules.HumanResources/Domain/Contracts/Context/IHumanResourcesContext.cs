using Core.Modules.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.Modules.HumanResources.Domain.Contracts.Context;

internal interface IHumanResourcesContext
{
    DbSet<Address> Addresses { get; set; }
    DbSet<Collaborator> Collaborators { get; set; }
    DbSet<Contract> Contracts { get; set; }
    DbSet<ContributionYear> ContributionYears { get; set; }
    DbSet<JobApplication> JobApplications { get; set; }
    DbSet<SocialLink> SocialLinks { get; set; }
    DbSet<WorkHour> WorkHours { get; set; }
    DbSet<State> States { get; init; }
    DatabaseFacade Database { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}