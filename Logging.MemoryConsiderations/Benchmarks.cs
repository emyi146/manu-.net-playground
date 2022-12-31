using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Logging.MemoryConsiderations;

/// <summary>
/// The results below show how the log without if, despite not logging due to min level
/// still allocates some unnecessary memory. An alternative is to use a custom log adapter. 
/// Serilog provides a more efficient mechanism (it implements a log adapter).
/// 
/// The property MinLevel is used to display what happens when the logs are not shown and when they are.
/// 
///  RESULTS: 
/// 
///   |                                       Method |      Mean |     Error |    StdDev |    Median |   Gen0 | Allocated |
///   |--------------------------------------------- |----------:|----------:|----------:|----------:|-------:|----------:|
///   |                                Log_WithoutIf | 17.378 ns | 0.1432 ns | 0.1118 ns | 17.416 ns |      - |         - |
///   |                                   Log_WithIf |  4.979 ns | 0.1024 ns | 0.0908 ns |  4.970 ns |      - |         - |
///   |                     Log_WithoutIf_WithParams | 61.902 ns | 0.8045 ns | 0.7525 ns | 61.614 ns | 0.0069 |      88 B |
///   |                        Log_WithIf_WithParams |  5.106 ns | 0.1197 ns | 0.1230 ns |  5.098 ns |      - |         - |
///   |                        LogAdapter_WithParams | 10.364 ns | 0.2308 ns | 0.5485 ns | 10.184 ns |      - |         - |
///   |                           Serilog_WithParams |  4.710 ns | 0.0736 ns | 0.0689 ns |  4.707 ns |      - |         - |
///   | LoggerMessageDefinition_WithoutIf_WithParams |  6.258 ns | 0.0888 ns | 0.0830 ns |  6.251 ns |      - |         - |
/// 
/// </summary>

[MemoryDiagnoser]
public class Benchmarks
{
    private const string _logMessageWithParams = "This is a log message with parameters {0}, {1}";
    private const string _logMessage = "This is a log message";

    [Params(LogLevel.Warning, LogLevel.Information)]
    public static LogLevel MinLevel;

    private ILoggerFactory _loggerFactory => LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(MinLevel);
        });

    private ILogger<Benchmarks> _logger => new Logger<Benchmarks>(_loggerFactory);
    private ILoggerAdapter<Benchmarks> _loggerAdapter => new LoggerAdapter<Benchmarks>(_logger);
    private Serilog.ILogger _serilog => new LoggerConfiguration()
            .MinimumLevel.Is(MinLevel == LogLevel.Warning ? LogEventLevel.Warning : LogEventLevel.Information)
            .WriteTo.Console()
            .CreateLogger();

    [Benchmark]
    public void Log_WithoutIf()
    {
        _logger.LogInformation(_logMessage);
    }

    [Benchmark]
    public void Log_WithIf()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(_logMessage);
        }
    }

    [Benchmark]
    public void Log_WithoutIf_WithParams()
    {
        _logger.LogInformation(_logMessageWithParams, 69, 96);
    }

    [Benchmark]
    public void Log_WithIf_WithParams()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(_logMessageWithParams, 69, 96);
        }
    }

    [Benchmark]
    public void LogAdapter_WithParams()
    {
        _loggerAdapter.LogInformation(_logMessageWithParams, 69, 96);
    }

    [Benchmark]
    public void Serilog_WithParams()
    {
        _serilog.Information(_logMessageWithParams, 69, 96);
    }

    [Benchmark]
    public void LoggerMessageDefinition_WithoutIf_WithParams()
    {
        _logger.LogBenchmarkMessage(69, 96);
    }

    [Benchmark]
    public void LoggerMessage_SourceGen_WithoutIf_WithParams()
    {
        _logger.LogBenchmarkMessageGen(69, 96);
    }
}
