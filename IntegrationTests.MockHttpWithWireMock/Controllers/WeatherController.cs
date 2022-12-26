using Microsoft.AspNetCore.Mvc;

namespace IntegrationTests.MockHttpWithMock.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private string _weatherProviderBaseUrl;
    private readonly HttpClient _httpClient;

    public WeatherController(IConfiguration configuration)
    {
        _weatherProviderBaseUrl = configuration.GetValue<string>("weatherProviderBaseUrl");
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_weatherProviderBaseUrl)
        };
    }

    /// <summary>
    /// Let's assume our integration tests cannot call this GET in a 
    /// deterministic way. We cannot trust on the real call, so we 
    /// have to mock it. We can use WireMock.NET
    /// </summary>
    [HttpGet("temperature")]
    public Task<string> Get()
    {
        // Call to external system we cannot write a test for
        return _httpClient.GetStringAsync("temperature-in-spain");
    }
}
