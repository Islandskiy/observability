using System.Diagnostics.Metrics;
using System.Reflection;
using Observability.API;
using Observability.API.Middlewares;
using OpenTelemetry.Metrics;
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

                            // Grafana Loki example
                            // configuration.WriteTo.GrafanaLoki("http://localhost:3100");
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
var serviceName = Assembly.GetExecutingAssembly().GetName().Name!;
builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
                                         {
                                             tracerProviderBuilder.AddJaegerExporter()
                                                                  //.AddConsoleExporter()
                                                                  .AddSource(serviceName)
                                                                  .SetResourceBuilder(
                                                                                      ResourceBuilder.CreateDefault()
                                                                                                     .AddService(serviceName: serviceName)
                                                                                                     .AddEnvironmentVariableDetector())
                                                                  .AddHttpClientInstrumentation()
                                                                  .AddAspNetCoreInstrumentation()
                                                                  .AddSqlClientInstrumentation();
                                         });

// Configure OpenTelemetry metrics
builder.Services.AddOpenTelemetryMetrics(meterProviderBuilder =>
                                         {
                                             // Allow export metrics from the specified meter
                                             meterProviderBuilder.AddMeter(serviceName);
                                             // Appends ASP.NET Core specific metrics (http_server_duration_ms)
                                             meterProviderBuilder.AddAspNetCoreInstrumentation();
                                             // Appends .NET runtime metrics (GC allocation, threads count, ...)
                                             meterProviderBuilder.AddRuntimeInstrumentation();
                                             // Allow export metrics in Prometheus style (pull approach via /metrics endpoint) 
                                             meterProviderBuilder.AddPrometheusExporter();
                                         });
// Register Meter instance as singleton according to best practices (https://docs.microsoft.com/en-us/dotnet/core/diagnostics/metrics-instrumentation#best-practices)
builder.Services.AddSingleton(new Meter(serviceName));

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
builder.Services.AddHttpClient<ServiceAClient>();
builder.Services.AddScoped<ServiceAClient>();

builder.Services.AddControllers();
builder.Services.AddSingleton<RequestDurationMiddleware>();

// Configure middleware workflow
var app = builder.Build();

// Add OpenTelemetry Prometheus scraping endpoint
app.UseOpenTelemetryPrometheusScrapingEndpoint();

// Metrics Histogram demo - request duration middleware
app.UseMiddleware<RequestDurationMiddleware>();

app.MapControllers();

app.Run();