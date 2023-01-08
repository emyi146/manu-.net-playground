using Microsoft.AspNetCore.Mvc;

namespace ManuHttpClient.HowToUse.Controllers;

[ApiController]
[Route("[controller]")]
public class GoogleController : ControllerBase
{
    private readonly InjectingHttpClientService _injectingHttpClientService;
    private readonly InjectingHttpClientFactoryService _injectingHttpClientFactoryService;

    public GoogleController(
        InjectingHttpClientService injectingHttpClientService,
        InjectingHttpClientFactoryService injectingHttpClientFactoryService)
    {
        _injectingHttpClientService = injectingHttpClientService;
        _injectingHttpClientFactoryService = injectingHttpClientFactoryService;
    }

    [HttpGet("httpclient")]
    public async Task<ActionResult> GetFromHttpClient()
    {
        return base.Content(await _injectingHttpClientService.Get(), "text/html");
    }

    [HttpGet("factory")]
    public async Task<ActionResult> GetFromHttpClientFactory()
    {
        return base.Content(await _injectingHttpClientFactoryService.Get(), "text/html");
    }
}

public class InjectingHttpClientService
{
    private readonly HttpClient _httpClient;

    public InjectingHttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<string> Get()
    {
        return await _httpClient.GetStringAsync("/");
    }
}

public class InjectingHttpClientFactoryService
{
    private readonly HttpClient _googleHttpClient;

    public InjectingHttpClientFactoryService(IHttpClientFactory httpClientFactory)
    {
        _googleHttpClient = httpClientFactory.CreateClient("google");
    }

    [HttpGet]
    public async Task<string> Get()
    {
        return await _googleHttpClient.GetStringAsync("/");
    }
}
