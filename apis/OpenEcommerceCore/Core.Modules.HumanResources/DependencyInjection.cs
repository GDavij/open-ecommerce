using Microsoft.Extensions.DependencyInjection;

namespace Core.Modules.HumanResources;

public static class DependencyInjection
{
    public static IServiceCollection AddHumanResourcesModule(this IServiceCollection services)
    {
        return services;
    }
}