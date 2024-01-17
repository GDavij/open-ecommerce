using System.Reflection;
using Core.Modules.HumanResources.Application.Http.Commands.CreateCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;
using Core.Modules.HumanResources.Domain.CustomConverters;
using Core.Modules.HumanResources.Infrastructure.Contexts;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Modules.HumanResources;

public static class DependencyInjection
{
    public static IServiceCollection RegisterHumanResourcesModule(this IServiceCollection services)
    {
        //MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });


        //Validators
        services.AddScoped<AbstractValidator<CreateCollaboratorCommand>, CreateCollaboratorCommandValidator>();
        
        //EfCore
        services.AddDbContext<IHumanResourcesContext, HumanResourcesContext>();
        
        return services;
    }
    
    public static IMvcBuilder AddHumanResourcesControllers(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(Assembly.GetExecutingAssembly())
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new CustomTimespanJsonConverter());
            });

        return mvcBuilder;
    }

    public static WebApplication RunHumanResourcesMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IHumanResourcesContext>();
        context.Database.Migrate();

        return app;
    }
}