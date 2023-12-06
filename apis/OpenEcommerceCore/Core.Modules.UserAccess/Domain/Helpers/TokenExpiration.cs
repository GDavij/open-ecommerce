using Core.Modules.UserAccess.Domain.Contracts.Providers;

namespace Core.Modules.UserAccess.Domain.Helpers;

internal static class TokenExpiration
{
    public static long OneDayFromNow(IDateTimeProvider dateTimeProvider) =>
        dateTimeProvider.UtcNowOffset.AddDays(1).ToUnixTimeSeconds();
}