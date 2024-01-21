using System.Reflection;
using Core.Modules.HumanResources;
using Core.Modules.Shared;
using Core.Modules.Stock;
using Core.Modules.UserAccess;
using MassTransit;
using Microsoft.OpenApi.Models;
using DependencyInjection = Core.Modules.HumanResources.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddUserAccessControllers()
    .AddStockControllers()
    .AddHumanResourcesControllers();
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Open Ecommerce Core Api",
        Description = "Open Ecommerce Core API Swagger Docs"
    });
});

// Http Context Accessor 
builder.Services.AddHttpContextAccessor();

//MassTransit
builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();

    cfg.AddUserAccessConsumers();
    
    cfg.UsingRabbitMq((ctx, conf) =>
    {
        var connectionStr = Environment.GetEnvironmentVariable("RABBITMQ_CORE_CONNECTION_STR")!;
        conf.Host(new Uri(connectionStr));
        
        conf.ConfigureEndpoints(ctx);
    });
});

builder.Services.RegisterSharedModule();
builder.Services.RegisterUserAccessModule();
builder.Services.RegisterStockModule();
builder.Services.RegisterHumanResourcesModule();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.RunStockMigrations();
    app.RunUserAccessMigrations();
    app.RunHumanResourcesMigrations();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();