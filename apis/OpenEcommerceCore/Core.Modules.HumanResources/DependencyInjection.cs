using System.Reflection;
using Core.Modules.HumanResources.Application.Http.Commands.AddContracts;
using Core.Modules.HumanResources.Application.Http.Commands.BreakContract;
using Core.Modules.HumanResources.Application.Http.Commands.CreateCollaborator;
using Core.Modules.HumanResources.Application.Http.Commands.DeleteCollaborator;
using Core.Modules.HumanResources.Application.Http.Commands.DeleteContract;
using Core.Modules.HumanResources.Application.Http.Commands.UpdateCollaborator;
using Core.Modules.HumanResources.Application.Http.Queries.GetCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.AddContracts;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.BreakContract;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteContract;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.UpdateCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries;
using Core.Modules.HumanResources.Domain.CustomConverters;
using Core.Modules.HumanResources.Domain.DynamicData;
using Core.Modules.HumanResources.Infrastructure.Contexts;
using FluentValidation;
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
        services.AddScoped<AbstractValidator<UpdateCollaboratorCommand>, UpdateCollaboratorCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteCollaboratorCommand>, DeleteCollaboratorCommandValidator>();
        services.AddScoped<AbstractValidator<GetCollaboratorQuery>, GetCollaboratorQueryValidator>();
        
        services.AddScoped<AbstractValidator<BreakContractCommand>, BreakContractCommandValidator>();
        services.AddScoped<AbstractValidator<AddContractsCommand>, AddContractsCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteContractCommand>, DeleteContractCommandValidator>(); 
        
        //Dynamic Data Resolvers
        services.AddScoped<ICurrentCollaboratorAsyncResolver, CurrentCollaboratorAsyncResolver>();
        
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