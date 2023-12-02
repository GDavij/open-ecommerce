namespace Core.Modules.UserAccess.Domain.Contracts.Services;

internal interface IAppConfigService
{
    public string GetEnvironmentVariable(string key);
}