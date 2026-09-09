using CoffeeNChill.Interface;
using CoffeeNChill.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

// Configure Azure Functions
// Configure Azure Functions with ASP.NET Core HTTP integration
builder.ConfigureFunctionsWebApplication();

// Register services for Dependency Injection
builder.Services.AddSingleton<ITableStorageService, TableStorageService>();
builder.Services.AddSingleton<IFileStorageService, FileStorageService>();

builder.Build().Run();
