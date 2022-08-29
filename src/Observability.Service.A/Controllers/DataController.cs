using Microsoft.AspNetCore.Mvc;

namespace Observability.Service.A.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;

    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        _logger.LogInformation("[Service A]. Log message");
        return "Result";
    }
}