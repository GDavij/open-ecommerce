using System.Reflection;
using Azure.Identity;
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
using Core.Modules.Stock.Application.Http.Queries.GetProduct;
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
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;
using Core.Modules.Stock.Domain.Contracts.Providers;
using Core.Modules.Stock.Infrastructure.Contexts;
using Core.Modules.Stock.Infrastructure.Providers;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

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
            cfg.UseCredential(new DefaultAzureCredential());

            Uri blobClientStorageConnectionString = new Uri(Environment.GetEnvironmentVariable("BLOB_CLIENT_STORAGE_CONNECTION_STRING")!);
            cfg.AddBlobServiceClient(blobClientStorageConnectionString);
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
        
        services.AddScoped<AbstractValidator<CreateMeasureUnitCommand>, CreateMeasureUnitCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteMeasureUnitCommand>, DeleteMeasureUnitCommandValidator>();
        services.AddScoped<AbstractValidator<UpdateMeasureUnitCommand>, UpdateMeasureUnitCommandValidator>();
        
        services.AddScoped<AbstractValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddScoped<AbstractValidator<DeleteProductCommand>, DeleteProductCommandValidator>();
        services.AddScoped<AbstractValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
        services.AddScoped<AbstractValidator<GetProductQuery>, GetProductQueryValidator>();
        
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