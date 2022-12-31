// You are doing .NET logging wrong. Let's fix it
// https://www.youtube.com/watch?v=bnVfrd3lRv8
// Check Benchmarks.cs for more details

using BenchmarkDotNet.Running;
using Logging.MemoryConsiderations;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole().SetMinimumLevel(LogLevel.Warning);
});

var logger = new Logger<Program>(loggerFactory);

// Note that despite the min log lever is warning,
// the logger.LogInformation allocates unnecessary memory
// which the GC has to take care of (idle cpu time)

LogWithoutIf(logger);
LogWithIf(logger);
Console.WriteLine("End");

// Logs nothing, but less efficient.
static void LogWithoutIf(ILogger logger)
{
    for (int i = 0; i < 69_000_000; i++)
    {
        logger.LogInformation("Random number {RandomNumber}", Random.Shared.Next());
    }
}

// More efficient
static void LogWithIf(ILogger logger)
{
    for (int i = 0; i < 69_000_000; i++)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Random number {RandomNumber}", Random.Shared.Next());
        }
    }
}

// Best approach, using adapter


// Benchmarks:
BenchmarkRunner.Run<Benchmarks>();