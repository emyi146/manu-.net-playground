using Microsoft.AspNetCore.Mvc;

namespace ApiVersioning.Controllers.v1;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class HelloController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public string Get()
    {
        return $"hello! Using version {HttpContext.GetRequestedApiVersion()}";
    }
}
