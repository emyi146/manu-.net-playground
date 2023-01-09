using System.Diagnostics;
using System.Text;

namespace MiddlewaresAndFilters.Middlewares;

public class SampleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SampleMiddleware> _logger;

    public SampleMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<SampleMiddleware>();
        // We can get services from DI: context.RequestServices.GetRequiredService<SomeService>()   
    }

    public async Task Invoke(HttpContext context)
    {
        // Before request
        context.Request.Headers["name"] = "John Smith";
        string traceId = Guid.NewGuid().ToString();
        _logger.LogTrace("Middleware, before method: request with traceid {traceId} started", traceId);
        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Middleware before request.\n"));
        Stopwatch stopwatch = Stopwatch.StartNew();

        // The request continues
        await _next(context);

        // After request
        _logger.LogDebug("Middleware, after method: request {traceId} took {elapsed} milliseconds", traceId, stopwatch.ElapsedMilliseconds);
        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Middleware after request.\n"));
    }
}
