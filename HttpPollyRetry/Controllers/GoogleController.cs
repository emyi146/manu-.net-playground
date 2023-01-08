using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;

namespace HttpPollyRetry.Controllers;

[ApiController]
[Route("[controller]")]
public class GoogleController : ControllerBase
{
    private const int MaxRetries = 3;
    private readonly AsyncRetryPolicy _retryNTimesPolicy;
    private readonly AsyncRetryPolicy _retryForeverPolicy;
    private readonly AsyncRetryPolicy _retryAfterWaitPolicy;
    private readonly AsyncRetryPolicy _retryAfterWaitWithExpBackoffPolicy;
    private readonly IHttpClientFactory _httpClientFactory;

    public GoogleController(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        var policyBuilder = Policy                                  // create a policy with the result string
            .Handle<HttpRequestException>(ex =>                     // which exceptions should handle, 
                ex.Message.Contains("fake request exception"));     // use lambda to filter the exception even more

        // retry max count
        _retryNTimesPolicy = policyBuilder.RetryAsync(MaxRetries);

        // retry forever (until success)
        _retryForeverPolicy = policyBuilder.RetryForeverAsync();

        // retry max count waiting 1 sec between retries
        _retryAfterWaitPolicy = policyBuilder.WaitAndRetryAsync(
            MaxRetries, times => TimeSpan.FromSeconds(1));

        // retry max count with incremental wait between retries:
        _retryAfterWaitWithExpBackoffPolicy =
            policyBuilder.WaitAndRetryAsync(
            MaxRetries, times => TimeSpan.FromMilliseconds(times * 1000));
    }

    /// <summary>
    /// Retry using Polly
    /// </summary>
    [HttpGet("polly")]
    public async Task<ActionResult> RetryWithPolly()
    {
        var httpClient = _httpClientFactory.CreateClient("google");

        var result = await _retryAfterWaitWithExpBackoffPolicy.ExecuteAsync(async () =>
        {
            // Force random errors
            if (Random.Shared.Next(1, 3) == 1)
            {
                throw new HttpRequestException("This is a fake request exception");
            }

            return await httpClient.GetStringAsync("/");
        });

        return base.Content(result, "text/html");
    }

    /// <summary>
    /// Retry using a custom implementation
    /// </summary>
    [HttpGet("manual")]
    public async Task<ActionResult> RetryManually()
    {
        var httpClient = _httpClientFactory.CreateClient("google");
        var retriesLeft = MaxRetries;
        string result = string.Empty;
        while (retriesLeft > 0)
        {
            try
            {

                // Force random errors
                if (Random.Shared.Next(1, 3) == 1)
                {
                    throw new HttpRequestException("This is a fake request exception");
                }

                result = await httpClient.GetStringAsync("/");
                Console.WriteLine("Request processed OK");
                break;
            }
            catch (Exception)
            {
                retriesLeft--;
                if (retriesLeft == 0)
                {
                    Console.WriteLine("No more retries => ERROR");
                    throw;
                }
                Console.WriteLine($"Retrying... retries left: {retriesLeft}");
            }
        }

        return base.Content(result, "text/html");
    }
}

