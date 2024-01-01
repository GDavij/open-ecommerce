using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Infrastructure.Contexts;

internal class UserAccessContext
: DbContext, IUserAccessContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Collaborator> Collaborators { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var connectionString = Environment.GetEnvironmentVariable("USER_ACCESS_POSTGRES_DATABASE")!;
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ClientMappings());
        modelBuilder.ApplyConfiguration(new CollaboratorMappings());
    }
}