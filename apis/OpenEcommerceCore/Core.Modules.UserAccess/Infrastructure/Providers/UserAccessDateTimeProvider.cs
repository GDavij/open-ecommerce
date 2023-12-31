using Core.Modules.UserAccess.Domain.Contracts.Providers;

namespace Core.Modules.UserAccess.Infrastructure.Providers;

internal class UserAccessDateTimeProvider : IUserAccessDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
}