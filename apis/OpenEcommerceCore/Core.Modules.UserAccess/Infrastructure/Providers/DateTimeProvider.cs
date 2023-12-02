using Core.Modules.UserAccess.Domain.Contracts.Providers;

namespace Core.Modules.UserAccess.Infrastructure.Providers;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
}