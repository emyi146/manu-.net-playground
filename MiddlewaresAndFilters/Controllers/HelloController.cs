using Microsoft.AspNetCore.Mvc;
using MiddlewaresAndFilters.Filters;

namespace MiddlewaresAndFilters.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [ServiceFilter(typeof(SampleFilter1))]
    [ServiceFilter(typeof(SampleFilter2))]
    [HttpGet]
    public string Get()
    {
        string name = Request.Headers["name"].ToString() ?? "unknown";  // Get header added by middleware
        return $"Hello {name}!!\n";
    }
}
