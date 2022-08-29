using Microsoft.AspNetCore.Mvc;
using Observability.API;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(builder.Configuration);
    
    // Serilog -> Sentry
    configuration.WriteTo.Sentry();
});
// Sentry
builder.WebHost
    .UseSentry(options =>
    {
        options.Dsn = "http://8293b68a3b3e41808f9490a11c9b4819@sentry.local:9000/2";
        options.Debug = true;
        options.TracesSampleRate = 1;
        options.Environment = builder.Environment.EnvironmentName;
    });

// Configure important OpenTelemetry settings, the console exporter, and instrumentation library
const string serviceName = "Observability.Api";
const string serviceVersion = "1.0.0";
builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        // .AddConsoleExporter()
        .AddJaegerExporter()
        .AddSource(serviceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation();
});

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
builder.Services.AddHttpClient<ServiceAClient>();
builder.Services.AddScoped<ServiceAClient>();

var app = builder.Build();

app.MapGet("/", ([FromServices]ILogger<Program> logger) =>
{
    logger.LogInformation("Test log message");
    return "Hello World!";
});

app.MapGet("/error", ([FromServices]ILogger<Program> logger) =>
{
    throw new Exception("Sentry test exception");
});

app.MapGet("/trace", async ([FromServices] ServiceAClient client) =>
{
    return await client.GetDataAsync();
});

app.Run();