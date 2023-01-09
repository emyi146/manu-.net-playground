using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace MiddlewaresAndFilters.Filters;

public class SampleFilter1 : ActionFilterAttribute
{
    private readonly ILogger<SampleFilter1> _logger;

    public SampleFilter1(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SampleFilter1>();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogTrace("Filter 1, before the method: OnActionExecuting");
        context.HttpContext.Response.Body.Write(Encoding.UTF8.GetBytes("Filter 1 before request.\n"));
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogTrace("Filter 1, after the method: OnActionExecuted");
        (context.Result as ObjectResult)!.Value += "Filter 1 after request.\n";
        base.OnActionExecuted(context);
    }
}
