using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Modules.UserAccess;

public static class DependencyInjection
{
    public static IServiceCollection RegisterUserAccessModule(this IServiceCollection services)
    {
        return services;
    }
}