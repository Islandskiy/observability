using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Observability.Service.A.Domain;

namespace Observability.Service.A.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;
    private readonly DataContext _context;

    public DataController(ILogger<DataController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("{id?}")]
    public async Task<string> Get(int? id = 1)
    {
        _logger.LogInformation("[Service A]. Get item with ID = {Id}", id);
        var item = await _context.DataItems.FindAsync(id);
        return item is not null ? item.Value : throw new Exception($"Item with ID = {id}");
    }
}