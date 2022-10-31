using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace Observability.API.Controllers;

public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;
    private readonly ServiceAClient _client;

    public ApiController(ILogger<ApiController> logger, ServiceAClient client)
    {
        _logger = logger;
        _client = client;
    }

    [HttpGet("/")]
    public string Get()
    {
        _logger.LogInformation("Test log message");
        // throw new Exception("koshmar");
        return "Hello World!";
    }

    [HttpGet("/error")]
    public void Error()
    {
        throw new Exception("Sentry test exception");
    }

    [HttpGet("/trace")]
    public async Task<string> Trace([FromServices] Tracer tracer)
    {
        _logger.LogInformation("Entered to root span");
        // Create child span
        using (var span = tracer.StartActiveSpan("prepare"))
        {
            span.SetAttribute("some-extra-info", "value " + DateTimeOffset.UtcNow);
            _logger.LogInformation("Inside child span");
            await Task.Delay(100);
        }
        return await _client.GetDataAsync();
    }
}