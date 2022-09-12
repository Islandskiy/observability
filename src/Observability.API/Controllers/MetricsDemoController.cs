using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;

namespace Observability.API.Controllers;

[Route("/metrics-demo")]
public class MetricsDemoController : ControllerBase
{
    private static readonly Random Random = new();
    private readonly Counter<long> _counter;

    public MetricsDemoController(Meter meter)
    {
        _counter = meter.CreateCounter<long>("just-a-counter");
    }

    [HttpGet("counter")]
    public string IncreaseCounter()
    {
        _counter.Add(1);
        return "OK";
    }
    
    [HttpGet("delay")]
    public async Task<string> Delay()
    {
        var ms = Random.Next(1000);
        await Task.Delay(ms);
        return "Delayed for " + ms + "ms";
    }
}