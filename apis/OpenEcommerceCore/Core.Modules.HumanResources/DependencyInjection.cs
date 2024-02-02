using System.Reflection;
using Azure.Identity;
using Core.Modules.HumanResources.Application.Http.Commands.Collaborators.CreateCollaborator;
using Core.Modules.HumanResources.Application.Http.Commands.Collaborators.DeleteCollaborator;
using Core.Modules.HumanResources.Application.Http.Commands.Collaborators.UpdateCollaborator;
using Core.Modules.HumanResources.Application.Http.Commands.Contracts.AddContracts;
using Core.Modules.HumanResources.Application.Http.Commands.Contracts.BreakContract;
using Core.Modules.HumanResources.Application.Http.Commands.Contracts.DeleteContract;
using Core.Modules.HumanResources.Application.Http.Commands.JobApplications.SendJobApplication;
using Core.Modules.HumanResources.Application.Http.Queries.Collaborators.GetCollaborator;
using Core.Modules.HumanResources.Application.Http.Queries.Collaborators.SearchCollaborators;
using Core.Modules.HumanResources.Application.Http.Queries.JobApplications.SearchJobApplications;
using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.CreateCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.DeleteCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.UpdateCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddContracts;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.BreakContract;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.DeleteContract;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.JobApplications.SendJobApplication;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Collaborators.GetCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Collaborators.SearchCollaborators;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.SearchJobApplications;
using Core.Modules.HumanResources.Domain.CustomConverters;
using Core.Modules.HumanResources.Domain.DynamicData;
using Core.Modules.HumanResources.Infrastructure.Contexts;
using Core.Modules.Shared.Domain.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        services.AddScoped<AbstractValidator<SearchCollaboratorsQuery>, SearchCollaboratorsQueryValidator>();
        
        services.AddScoped<AbstractValidator<BreakContractCommand>, BreakContractCommandValidator>();
        services.AddScoped<AbstractValidator<AddContractsCommand>, AddContractsCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteContractCommand>, DeleteContractCommandValidator>();

        services.AddScoped<AbstractValidator<SendJobApplicationCommand>, SendJobApplicationCommandValidator>();
        services.AddScoped<AbstractValidator<SearchJobApplicationsQuery>, SearchJobApplicationsQueryValidator>();
       
        services.AddAzureClients(cfg => 
        {
            string azBlobClientStorageConnection = Environment.GetEnvironmentVariable(SharedConnectionStringEnvironmentVariableName.AzureBlobStorage)!;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production)
            {
                Uri blobClientStorageConnectionString = new Uri(azBlobClientStorageConnection);
                cfg.UseCredential(new DefaultAzureCredential());
                cfg.AddBlobServiceClient(blobClientStorageConnectionString);
            }
            else
            {
                cfg.AddBlobServiceClient(azBlobClientStorageConnection);
            }
        });
        
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