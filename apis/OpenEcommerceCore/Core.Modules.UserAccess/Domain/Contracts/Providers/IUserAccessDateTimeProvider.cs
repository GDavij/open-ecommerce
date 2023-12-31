namespace Core.Modules.UserAccess.Domain.Contracts.Providers;

public interface IUserAccessDateTimeProvider
{
    DateTime UtcNow { get; }
    
    DateTimeOffset UtcNowOffset { get; }
}