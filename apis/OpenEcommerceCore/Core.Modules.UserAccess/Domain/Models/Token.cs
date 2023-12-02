using System.Text.Json.Serialization;

namespace Core.Modules.UserAccess.Domain.Models;

internal class Token
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public ETokenType Type { get; set; }
    public long Exp { get; set; }
    
    // Constructor only for deserialize JWT Tokens 
    [JsonConstructor]
    public Token()
    {}
    
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
        byte[] password,
        ETokenType type,
        long exp)
    {
        string base64Password = Convert.ToBase64String(password);
        
        return new Token(
            id,
            base64Password,
            type,
            exp);
    }
}

internal enum ETokenType
{
    Client,
    Collaborator
}