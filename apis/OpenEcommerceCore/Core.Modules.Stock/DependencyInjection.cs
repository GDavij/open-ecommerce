using System.Reflection;
using Azure.Identity;
using Core.Modules.Shared.Domain.Constants;
using Core.Modules.Stock.Application.Http.Commands;
using Core.Modules.Stock.Application.Http.Commands.AddImageToProduct;
using Core.Modules.Stock.Application.Http.Commands.CreateBrand;
using Core.Modules.Stock.Application.Http.Commands.CreateMeasureUnit;
using Core.Modules.Stock.Application.Http.Commands.CreateProduct;
using Core.Modules.Stock.Application.Http.Commands.DeleteBrand;
using Core.Modules.Stock.Application.Http.Commands.DeleteMeasureUnit;
using Core.Modules.Stock.Application.Http.Commands.DeleteProduct;
using Core.Modules.Stock.Application.Http.Commands.RemoveImageFromProduct;
using Core.Modules.Stock.Application.Http.Commands.UpdateMeasureUnit;
using Core.Modules.Stock.Application.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Application.Http.Queries.GetBrand;
using Core.Modules.Stock.Application.Http.Queries.GetProduct;
using Core.Modules.Stock.Application.Http.Queries.SearchBrand;
using Core.Modules.Stock.Application.Http.Queries.SearchProduct;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchProduct;
using Core.Modules.Stock.Domain.Contracts.Providers;
using Core.Modules.Stock.Infrastructure.Contexts;
using Core.Modules.Stock.Infrastructure.Providers;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Modules.Stock;

public static class DependencyInjection
{
    public static IServiceCollection RegisterStockModule(this IServiceCollection services)
    {
        //MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        //Azure Storage Configuration
        services.AddAzureClients(cfg =>
        {
            string azBlobClientStorageConnection = Environment.GetEnvironmentVariable(SharedConnectionStringEnvironmentVariableName.AzureBlobStorage);
            
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

        //Db Contexts
        services.AddDbContext<IStockContext, StockContext>();
        
        //Providers
        services.AddScoped<IStockDateTimeProvider, StockDateTimeProvider>();

        //Validators 
        services.AddScoped<AbstractValidator<AddImageToProductCommand>, AddImageToProductCommandValidator>();
        services.AddScoped<AbstractValidator<RemoveImageFromProductCommand>, RemoveImageFromProductCommandValidator>();

        services.AddScoped<AbstractValidator<CreateBrandCommand>, CreateBrandCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteBrandCommand>, DeleteBrandCommandValidator>();
        services.AddScoped<AbstractValidator<UpdateBrandCommand>, UpdateBrandCommandValidator>();
        services.AddScoped<AbstractValidator<GetBrandQuery>, GetBrandQueryValidator>();
        services.AddScoped<AbstractValidator<SearchBrandQuery>, SearchBrandQueryValidator>();

        services.AddScoped<AbstractValidator<CreateMeasureUnitCommand>, CreateMeasureUnitCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteMeasureUnitCommand>, DeleteMeasureUnitCommandValidator>();
        services.AddScoped<AbstractValidator<UpdateMeasureUnitCommand>, UpdateMeasureUnitCommandValidator>();

        services.AddScoped<AbstractValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteProductCommand>, DeleteProductCommandValidator>();
        services.AddScoped<AbstractValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
        services.AddScoped<AbstractValidator<GetProductQuery>, GetProductQueryValidator>();
        services.AddScoped<AbstractValidator<SearchProductQuery>, SearchProductQueryValidator>();
        
        
        return services;
    }

    public static WebApplication RunStockMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IStockContext>();
        context.Database.Migrate();

        return app;
    }
}