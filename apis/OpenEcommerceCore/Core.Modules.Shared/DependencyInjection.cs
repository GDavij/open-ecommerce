using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Modules.Shared;

public static class DependencyInjection
{
    public static IServiceCollection RegisterSharedModule(this IServiceCollection services)
    {
       //Services
       services.AddSingleton<IAppConfigService, AppConfigService>();

       return services;
    }
}