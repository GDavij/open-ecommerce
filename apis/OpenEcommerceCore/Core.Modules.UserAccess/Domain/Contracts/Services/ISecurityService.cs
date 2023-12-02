using Core.Modules.UserAccess.Domain.Models;

namespace Core.Modules.UserAccess.Domain.Contracts.Services;

internal interface ISecurityService
{
    // Refactor to Task<string> if possible, high processing Cost
    Task<byte[]> DerivePassword(string password, byte[] userSecurityKey, CancellationToken cancellationToken);

    byte[] GenerateSecurityKey();

    string EncodeToken(Token token);

    bool TryParseEncodedToken(string encodedToken, out Token token);
}