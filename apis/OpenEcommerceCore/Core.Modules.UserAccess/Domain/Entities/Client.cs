namespace Core.Modules.UserAccess.Domain.Entities;

internal sealed class Client
{
    public Guid Id { get; init; }
    public Guid ClientModuleId { get; init; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string SecurityKey { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastLogin { get; set; }
    
    private Client(
        Guid id,
        Guid clientModuleId,
        string email,
        string password,
        string securityKey,
        DateTime createdAt,
        DateTime lastLogin)
    {
        Id = id;
        ClientModuleId = clientModuleId;
        Email = email;
        Password = password;
        SecurityKey = securityKey;
        CreatedAt = createdAt;
        LastLogin = lastLogin;
    }

    public static Client Create(
        Guid id,
        Guid clientModuleId,
        string email,
        string password,
        string securityKey,
        DateTime createdAt,
        DateTime lastLogin)
    {
        return new Client(
            id,
            clientModuleId,
            email,
            password,
            securityKey,
            createdAt,
            lastLogin);
    }
}