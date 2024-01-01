using System.Reflection;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Infrastructure.Contexts;
using Core.Modules.UserAccess.Infrastructure.Providers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Modules.UserAccess;

public static class DependencyInjection
{
    public static IServiceCollection RegisterUserAccessModule(this IServiceCollection services)
    {
        //MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        //MassTransit
        services.AddMassTransit(cfg =>
        {
            cfg.UsingInMemory();
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddConsumers(Assembly.GetExecutingAssembly());
        });

        //Db Contexts
        services.AddScoped<IUserAccessContext, UserAccessContext>();

        //Providers
        services.AddScoped<IUserAccessDateTimeProvider, UserAccessDateTimeProvider>();

        //Services
        services.AddScoped<ISecurityService, SecurityService>();

        return services;
    }

    public static WebApplication RunUserAccessMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IUserAccessContext>();
        context.Database.Migrate();

        return app;
    }
}