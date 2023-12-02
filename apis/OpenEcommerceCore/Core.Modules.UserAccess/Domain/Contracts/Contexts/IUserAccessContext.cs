using Core.Modules.UserAccess.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Core.Modules.UserAccess.Domain.Contracts.Contexts;

internal interface IUserAccessContext 
{
    DbSet<Client> Clients { get; set; }
    DbSet<Collaborator> Collaborators { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}