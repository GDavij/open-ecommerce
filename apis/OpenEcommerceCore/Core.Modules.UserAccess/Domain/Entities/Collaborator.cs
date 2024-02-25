using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.UserAccess.Domain.Entities;

internal sealed class Collaborator
{
    public Guid Id { get; init; }
    public Guid? CollaboratorModuleId { get; init; }
    public string Email { get; set; }
    public byte[] Password { get; set; }
    public byte[] SecurityKey { get; set; }
    public List<ECollaboratorSector> Sectors { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastLogin { get; set; }
    public bool IsAdmin { get; init; }
    public bool Deleted { get; set; }
    
    private Collaborator(
        Guid id,
        Guid? collaboratorModuleId,
        string email,
        byte[] password,
        byte[] securityKey,
        List<ECollaboratorSector> sectors,
        DateTime createdAt,
        DateTime lastLogin,
        bool isAdmin,
        bool deleted)
    {
        Id = id;
        CollaboratorModuleId = collaboratorModuleId;
        Email = email;
        Password = password;
        SecurityKey = securityKey;
        Sectors = sectors;
        CreatedAt = createdAt;
        LastLogin = lastLogin;
        IsAdmin = isAdmin;
        Deleted = deleted;
    }

    public static Collaborator Create(
        Guid id,
        Guid? collaboratorModuleId,
        string email,
        byte[] password,
        byte[] securityKey,
        bool isAdmin,
        List<ECollaboratorSector> sectors)
    {
        return new Collaborator(
            id,
            collaboratorModuleId,
            email,
            password,
            securityKey,
            sectors,
            DateTime.UtcNow,
            DateTime.UtcNow,
            isAdmin,
            false);
    }

    public void Delete()
    {
        Deleted = true;
    }
}