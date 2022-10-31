using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Observability.API.Middlewares;

public sealed class ServerMetricsMiddleware : IMiddleware
{
    public const string DurationInstrumentName = "observability-api---request-duration";
    public const string RequestCounterInstrumentName = "observability-api---total-requests";
    private readonly Histogram<long> _requestDurationHistogram;
    private readonly Counter<long> _requestCounter;

    public ServerMetricsMiddleware(Meter meter)
    {
        _requestDurationHistogram = meter.CreateHistogram<long>(DurationInstrumentName);
        _requestCounter = meter.CreateCounter<long>(RequestCounterInstrumentName);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _requestCounter.Add(1);
        
        var stopwatch = Stopwatch.StartNew();
        await next(context);
        _requestDurationHistogram.Record(stopwatch.ElapsedMilliseconds);
    }
}