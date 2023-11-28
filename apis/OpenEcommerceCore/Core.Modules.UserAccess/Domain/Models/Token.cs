namespace Core.Modules.UserAccess.Domain.Models;

internal class Token
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public ETokenType Type { get; set; }
    public long Exp { get; set; }

    private Token(
        Guid id,
        string password,
        ETokenType type,
        long exp)
    {
        Id = id;
        Password = password;
        Type = type;
        Exp = exp;
    }

    public static Token Create(
        Guid id,
        string password,
        ETokenType type,
        long exp)
    {
        return new Token(
            id,
            password,
            type,
            exp);
    }
}

internal enum ETokenType
{
    Client,
    Collaborator
}