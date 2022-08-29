namespace Observability.API;

public class ServiceAClient
{
    private readonly ILogger<ServiceAClient> _logger;
    private readonly HttpClient _httpClient;

    public ServiceAClient(ILogger<ServiceAClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<string> GetDataAsync()
    {
        return await _httpClient.GetStringAsync("https://localhost:7230/data");
    }
}