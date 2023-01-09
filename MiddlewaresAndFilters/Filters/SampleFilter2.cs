using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace MiddlewaresAndFilters.Filters;

public class SampleFilter2 : IAsyncActionFilter
{
    private readonly ILogger<SampleFilter1> _logger;

    public SampleFilter2(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SampleFilter1>();
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogTrace("Filter 2, before the method: OnActionExecutionAsync");
        context.HttpContext.Response.Body.Write(Encoding.UTF8.GetBytes("Filter 2 before request.\n"));

        var actionExecuted = await next();

        _logger.LogTrace("Filter 2, after the method: OnActionExecutionAsync");
        (actionExecuted.Result as ObjectResult)!.Value += "Filter 2 after request.\n";
    }
}
