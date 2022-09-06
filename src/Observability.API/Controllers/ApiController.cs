using Microsoft.AspNetCore.Mvc;

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
        return "Hello World!";
    }

    [HttpGet("/error")]
    public void Error()
    {
        throw new Exception("Sentry test exception"); 
    }
    [HttpGet("/trace")]
    public async Task<string> Trace()
    {
        return await _client.GetDataAsync(); 
    }
}