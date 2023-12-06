using Core.Modules.Shared.Domain.Models;

namespace Core.Modules.Shared.Domain.ResultObjects;

public class AuthenticationResult
{
    public bool IsAuthenticated { get; }
    public Identity? Identity { get; }

    private AuthenticationResult(bool isAuthenticated, Identity? identity)
    {
        IsAuthenticated = isAuthenticated;
        Identity = identity;
    }

    public static AuthenticationResult IsAuthenticatedWithIdentity(Identity identity)
    {
        var result = new AuthenticationResult(true, identity);
        return result;
    }

    public static AuthenticationResult NotAuthenticated()
    {
        var result = new AuthenticationResult(false, null);
        return result;
    }
};