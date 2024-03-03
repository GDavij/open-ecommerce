using System.Reflection;
using Core.Modules.UserAccess.Application.Http.Commands.Administrators.CreateAdministrator;
using Core.Modules.UserAccess.Application.Http.Commands.Administrators.DeleteAdministrator;
using Core.Modules.UserAccess.Application.Http.Commands.Administrators.UpdateAdministrator;
using Core.Modules.UserAccess.Application.Http.Commands.Collaborators.CreateCollaboratorSession;
using Core.Modules.UserAccess.Application.Http.Queries.Administrators.ListAdministrators;
using Core.Modules.UserAccess.Application.Messaging.Commands.Auth;
using Core.Modules.UserAccess.Application.Messaging.Commands.Collaborators;
using Core.Modules.UserAccess.Application.Messaging.Queries.Administrators;
using Core.Modules.UserAccess.Application.Messaging.Queries.Collaborators;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.CreateAdministrator;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.DeleteAdministrator;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.UpdateAdministrator;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Collaborators.CreateCollaboratorSession;
using Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Administrators.ListAdministrators;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.DynamicData;
using Core.Modules.UserAccess.Infrastructure.Contexts;
using Core.Modules.UserAccess.Infrastructure.Providers;
using FluentValidation;
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

        //Db Contexts
        services.AddScoped<IUserAccessContext, UserAccessContext>();

        //Providers
        services.AddScoped<IUserAccessDateTimeProvider, UserAccessDateTimeProvider>();

        //Services
        services.AddScoped<ISecurityService, SecurityService>();
       
        //Dynamic Data
        services.AddScoped<ICurrentCollaboratorAsyncResolver, CurrentCollaboratorAsyncResolver>();
        
        //Validators 
        services.AddScoped<AbstractValidator<CreateCollaboratorSessionCommand>, CreateCollaboratorSessionCommandValidator>();

        services.AddScoped<AbstractValidator<CreateAdministratorCommand>, CreateAdministratorCommandValidator>();
        services.AddScoped<AbstractValidator<UpdateAdministratorCommand>, UpdateAdministratorCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteAdministratorCommand>, DeleteAdministratorCommandValidator>();

        services.AddScoped<AbstractValidator<ListAdministratorsQuery>, ListAdministratorsQueryValidator>();
            
        return services;
    }

    public static IBusRegistrationConfigurator AddUserAccessConsumers(this IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<CreateCollaboratorCommandHandler>();
        cfg.AddConsumer<UpdateCollaboratorCommandHandler>();
        cfg.AddConsumer<DeleteCollaboratorCommandHandler>();
        
        cfg.AddConsumer<AuthenticateCollaboratorForSectorCommandHandler>();
        
        cfg.AddConsumer<GetCollaboratorIsAdminQueryHandler>();
        cfg.AddConsumer<GetCollaboratorIsDeletedQueryHandler>();
        cfg.AddConsumer<GetDeletedCollaboratorsIdsQueryHandler>();
        cfg.AddConsumer<GetAdministratorsIdsQueryHandler>();
        cfg.AddConsumer<GetCollaboratorsIdsFromSectorQueryHandler>();

        cfg.AddConsumer<RemoveCollaboratorSectorCommandHandler>();
        cfg.AddConsumer<AddCollaboratorSectorsCommandHandler>();
        
        return cfg;
    }

    public static IMvcBuilder AddUserAccessControllers(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(Assembly.GetExecutingAssembly());

        return mvcBuilder;
    }

    public static WebApplication RunUserAccessMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IUserAccessContext>();
        context.Database.Migrate();

        return app;
    }
}