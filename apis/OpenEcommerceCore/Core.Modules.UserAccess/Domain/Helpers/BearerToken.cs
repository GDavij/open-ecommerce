namespace Core.Modules.UserAccess.Domain.Helpers;

internal static class BearerToken
{
    public static string ParseAndGetEncodedToken(string authorization)
    => authorization.Split(' ')[1];
}