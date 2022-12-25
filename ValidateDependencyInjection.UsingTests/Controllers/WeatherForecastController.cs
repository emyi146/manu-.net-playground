using Microsoft.AspNetCore.Mvc;
using ValidateDependencyInjection.Mvc.Testing.Services;

namespace ValidateDependencyInjection.Mvc.Testing.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    private readonly ISomeService _someService;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        ISomeService someService,
        ILogger<WeatherForecastController> logger)
    {
        _someService = someService;
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        _someService.Run();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
