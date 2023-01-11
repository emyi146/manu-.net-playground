using Microsoft.AspNetCore.Mvc;

namespace ApiVersioning.Controllers.v2;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class HelloController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("2.0")]
    public string Get()
    {
        return "hello 1";
    }
}
