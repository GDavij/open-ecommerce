using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.Shared.Domain.Services;

internal class AppConfigService : IAppConfigService
{
    public string GetEnvironmentVariable(string key)
    {
        return Environment.GetEnvironmentVariable(key);
    }
}