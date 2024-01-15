using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Infrastructure.Contexts;

internal class HumanResourcesContext
    : DbContext, IHumanResourcesContext
{
    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<ContributionYear> ContributionYears { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<SocialLink> SocialLinks { get; set; }
    public DbSet<WorkHour> WorkHours { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var connectionStr = Environment.GetEnvironmentVariable("HUMAN_RESOURCES_POSTGRES_DATABASE");
        optionsBuilder.UseNpgsql(connectionStr);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AddressMappings());
        modelBuilder.ApplyConfiguration(new CollaboratorMappings());
        modelBuilder.ApplyConfiguration(new ContractMappings());
        modelBuilder.ApplyConfiguration(new ContributionYearMappings());
    }
}