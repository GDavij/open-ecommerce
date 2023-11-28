using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.UserAccess.Domain.Entities;

internal sealed class Collaborator
{
    public Guid Id { get; init; }
    public Guid CollaboratorModuleId { get; init; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string SecurityKey { get; set; }
    public ECollaboratorSector Sector { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastLogin { get; set; }
    
    private Collaborator(
        Guid id,
        Guid collaboratorModuleId,
        string email,
        string password,
        string securityKey,
        ECollaboratorSector sector,
        DateTime createdAt,
        DateTime lastLogin)
    {
        Id = id;
        CollaboratorModuleId = collaboratorModuleId;
        Email = email;
        Password = password;
        SecurityKey = securityKey;
        Sector = sector;
        CreatedAt = createdAt;
        LastLogin = lastLogin;
    }

    public static Collaborator Create(
        Guid id,
        Guid collaboratorModuleId,
        string email,
        string password,
        string securityKey,
        ECollaboratorSector sector,
        DateTime createdAt,
        DateTime lastLogin)
    {
        return new Collaborator(
            id,
            collaboratorModuleId,
            email,
            password,
            securityKey,
            sector,
            createdAt,
            lastLogin);
    }
}