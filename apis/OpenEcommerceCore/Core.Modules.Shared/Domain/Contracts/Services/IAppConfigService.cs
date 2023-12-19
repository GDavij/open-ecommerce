namespace Core.Modules.Shared.Domain.Contracts.Services;

public interface IAppConfigService
{
    public string GetEnvironmentVariable(string key);
}
