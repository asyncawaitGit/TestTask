using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using TestConsoleApp;
using TestDLL;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/test-sms-console-app-.log", 
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,   
        rollOnFileSizeLimit: true,
        fileSizeLimitBytes: 10_000_000)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var grpcUrl = builder.Configuration["Grpc:Address"]
    ?? throw new InvalidOperationException("Grpc:Address not configured");

builder.Services.AddGrpcApiClient(grpcUrl);
builder.Services.AddSingleton<MenuConsoleService>();
builder.Services.AddSingleton<DbInitializer>();

var host = builder.Build();

await host.Services
    .GetRequiredService<DbInitializer>()
    .InitAsync();

await host.Services
    .GetRequiredService<MenuConsoleService>()
    .RunAsync();