using Microsoft.AspNetCore.Mvc;

namespace CancellationTokenExample.Controllers;

/// <summary>
/// https://www.youtube.com/watch?v=b5dyPJ3zyRg
/// Stop wasting server resources by properly using CancellationToken in .NET
/// 
/// Lesson: pass the cancellation token for the whole cascade of calls so that if the caller cancels the call,
/// all the work is cancelled. Most libraries allow to pass a cancellation token (EF, Dapper, etc) 
/// </summary>
[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    [HttpGet("cancellation")]
    public async Task<IActionResult> UnderPerformingQuery_WithCancellationToken(CancellationToken token)
    {
        try
        {
            Console.WriteLine("Underperforming query started");
            await Task.Delay(10000, token);
            Console.WriteLine("Underperforming query finished");
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("Task was cancelled and exception handled", e);
        }
        return Ok();
    }

    [HttpGet("")]
    public async Task<IActionResult> UnderPerformingQuery_WithoutCancellation()
    {
        Console.WriteLine("Underperforming query started");
        await Task.Delay(10000);
        Console.WriteLine("Underperforming query finished");
        return Ok();
    }
}
