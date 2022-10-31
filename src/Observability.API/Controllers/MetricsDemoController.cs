using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using Observability.API.Metrics;

namespace Observability.API.Controllers;

[Route("/metrics-demo")]
public class MetricsDemoController : ControllerBase
{
    private readonly JustACounter _justACounter;
    private static readonly Random Random = new();

    public MetricsDemoController(JustACounter justACounter)
    {
        _justACounter = justACounter;
    }

    [HttpGet("counter")]
    public string IncreaseCounter()
    {
        _justACounter.Increment();
        return "OK";
    }
    
    [HttpGet("delay")]
    public async Task<string> Delay()
    {
        var ms = Random.Next(1000, 2000);
        await Task.Delay(ms);
        return "Delayed for " + ms + "ms";
    }
}