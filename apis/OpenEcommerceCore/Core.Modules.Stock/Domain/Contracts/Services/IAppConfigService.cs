namespace Core.Modules.Stock.Domain.Contracts.Services;

internal interface IAppConfigService
{
    public string GetEnvironmentVariable(string key);
}