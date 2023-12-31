using Core.Modules.UserAccess.Domain.Contracts.Providers;

namespace Core.Modules.UserAccess.Domain.Helpers;

internal static class TokenExpiration
{
    public static long OneDayFromNow(IUserAccessDateTimeProvider userAccessDateTimeProvider) =>
        userAccessDateTimeProvider.UtcNowOffset.AddDays(1).ToUnixTimeSeconds();
}