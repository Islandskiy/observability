using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Observability.API.Middlewares;

public sealed class RequestDurationMiddleware : IMiddleware
{
    private readonly Histogram<long> _requestDurationHistogram;
    
    public RequestDurationMiddleware(Meter meter)
    {
        _requestDurationHistogram = meter.CreateHistogram<long>("observability-api---request-duration");
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        await next(context);
        _requestDurationHistogram.Record(stopwatch.ElapsedMilliseconds);
    }
}