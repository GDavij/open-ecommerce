using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.UserAccess.Domain.Entities;

internal sealed class Collaborator
{
    public Guid Id { get; init; }
    public Guid CollaboratorModuleId { get; init; }
    public string Email { get; set; }
    public byte[] Password { get; set; }
    public byte[] SecurityKey { get; set; }
    public ECollaboratorSector Sector { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastLogin { get; set; }
    public bool Deleted { get; set; }
    
    private Collaborator(
        Guid id,
        Guid collaboratorModuleId,
        string email,
        byte[] password,
        byte[] securityKey,
        ECollaboratorSector sector,
        DateTime createdAt,
        DateTime lastLogin,
        bool deleted)
    {
        Id = id;
        CollaboratorModuleId = collaboratorModuleId;
        Email = email;
        Password = password;
        SecurityKey = securityKey;
        Sector = sector;
        CreatedAt = createdAt;
        LastLogin = lastLogin;
        Deleted = deleted;
    }

    public static Collaborator Create(
        Guid id,
        Guid collaboratorModuleId,
        string email,
        byte[] password,
        byte[] securityKey,
        ECollaboratorSector sector)
    {
        return new Collaborator(
            id,
            collaboratorModuleId,
            email,
            password,
            securityKey,
            sector,
            DateTime.UtcNow,
            DateTime.UtcNow,
            false);
    }
}