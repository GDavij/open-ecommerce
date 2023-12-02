namespace Core.Modules.UserAccess.Domain.Contracts.Providers;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    
    DateTimeOffset UtcNowOffset { get; }
}