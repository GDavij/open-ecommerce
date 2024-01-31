using System.Reflection;
using Core.Modules.UserAccess.Application.Http.Commands.CreateCollaboratorSession;
using Core.Modules.UserAccess.Application.Messaging.Commands.AddCollaboratorSectors;
using Core.Modules.UserAccess.Application.Messaging.Commands.AuthenticateCollaboratorForSector;
using Core.Modules.UserAccess.Application.Messaging.Commands.CreateCollaborator;
using Core.Modules.UserAccess.Application.Messaging.Commands.DeleteCollaborator;
using Core.Modules.UserAccess.Application.Messaging.Commands.GetAdministratorsIds;
using Core.Modules.UserAccess.Application.Messaging.Commands.GetCollaboratorIsAdmin;
using Core.Modules.UserAccess.Application.Messaging.Commands.GetCollaboratorIsDeleted;
using Core.Modules.UserAccess.Application.Messaging.Commands.GetCollaboratorsIdsFromSector;
using Core.Modules.UserAccess.Application.Messaging.Commands.GetDeletedCollaboratorsIds;
using Core.Modules.UserAccess.Application.Messaging.Commands.RemoveCollaboratorSector;
using Core.Modules.UserAccess.Application.Messaging.Commands.UpdateCollaborator;
using Core.Modules.UserAccess.Application.Services;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
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
        
        //Validators 
        services.AddScoped<AbstractValidator<CreateCollaboratorSessionCommand>, CreateCollaboratorSessionCommandValidator>();
        
        return services;
    }

    public static IBusRegistrationConfigurator AddUserAccessConsumers(this IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<CreateCollaboratorCommandHandler>();
        cfg.AddConsumer<UpdateCollaboratorCommandHandler>();
        cfg.AddConsumer<DeleteCollaboratorCommandHandler>();
        
        cfg.AddConsumer<AuthenticateCollaboratorForSectorCommandHandler>();
        
        cfg.AddConsumer<GetCollaboratorIsAdminCommandHandler>();
        cfg.AddConsumer<GetCollaboratorIsDeletedCommandHandler>();
        cfg.AddConsumer<GetDeletedCollaboratorsIdsCommandHandler>();
        cfg.AddConsumer<GetAdministratorsIdsCommandHandler>();
        cfg.AddConsumer<GetCollaboratorsIdsFromSectorCommandHandler>();

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