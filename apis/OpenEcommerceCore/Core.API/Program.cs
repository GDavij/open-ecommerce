using System.Reflection;
using Core.Modules.Shared;
using Core.Modules.Stock;
using Core.Modules.UserAccess;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddUserAccessControllers()
    .AddStockControllers();
    
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

builder.Services.RegisterSharedModule();
builder.Services.RegisterUserAccessModule();
builder.Services.RegisterStockModule();

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
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();