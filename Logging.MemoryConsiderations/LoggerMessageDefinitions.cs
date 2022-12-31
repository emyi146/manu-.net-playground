using Microsoft.Extensions.Logging;

namespace Logging.MemoryConsiderations;
public static class LoggerMessageDefinitions
{
    private static readonly Action<ILogger, int, int, Exception?> BenchmarkLogMessageDefinition =
        LoggerMessage.Define<int, int>(LogLevel.Information, 0,
            "This is a log message with parameters {First}, {Second}");

    public static void LogBenchmarkMessage(this ILogger logger, int first, int second)
    {
        BenchmarkLogMessageDefinition(logger, first, second, null);
    }
}

public static partial class LoggerMessageDefinitionsGen
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "This is a log message with parameters {First}, {Second}")]
    public static partial void LogBenchmarkMessageGen(this ILogger logger, int first, int second);
}