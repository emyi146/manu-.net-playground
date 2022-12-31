using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Logging.MemoryConsiderations;

/// <summary>
/// The results below show how the log without if, despite not logging due to min level
/// still allocates some unnecessary memory. An alternative is to use a custom log adapter. 
/// Serilog provides a more efficient mechanism (it implements a log adapter).
/// 
///  RESULTS: 
/// 
///   |                   Method |      Mean |     Error |    StdDev |   Gen0 | Allocated |
///   |------------------------- |----------:|----------:|----------:|-------:|----------:|
///   |            Log_WithoutIf | 19.424 ns | 0.2947 ns | 0.2756 ns |      - |         - |
///   |               Log_WithIf |  4.961 ns | 0.0353 ns | 0.0330 ns |      - |         - |
///   | Log_WithoutIf_WithParams | 59.849 ns | 0.4154 ns | 0.3243 ns | 0.0069 |      88 B |
///   |    Log_WithIf_WithParams |  4.644 ns | 0.1143 ns | 0.1173 ns |      - |         - |
///   |    LogAdapter_WithParams | 10.114 ns | 0.2229 ns | 0.2190 ns |      - |         - |
///   |       Serilog_WithParams |  4.764 ns | 0.1083 ns | 0.1013 ns |      - |         - |
/// 
/// </summary>

[MemoryDiagnoser]
public class Benchmarks
{
    private const string LogMessageWithParams = "This is a log message with parameters {0}, {1}";
    private const string LogMessage = "This is a log message";

    private readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole().SetMinimumLevel(LogLevel.Warning);
    });

    private readonly ILogger<Benchmarks> _logger;
    private readonly ILoggerAdapter<Benchmarks> _loggerAdapter;
    private readonly Serilog.ILogger _serilog = new LoggerConfiguration()
        .MinimumLevel.Warning()
        .WriteTo.Console()
        .CreateLogger();

    public Benchmarks()
    {
        _logger = new Logger<Benchmarks>(_loggerFactory);
        _loggerAdapter = new LoggerAdapter<Benchmarks>(_logger);
    }

    [Benchmark]
    public void Log_WithoutIf()
    {
        _logger.LogInformation(LogMessage);
    }


    [Benchmark]
    public void Log_WithIf()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(LogMessage);
        }
    }

    [Benchmark]
    public void Log_WithoutIf_WithParams()
    {
        _logger.LogInformation(LogMessageWithParams, 69, 96);
    }

    [Benchmark]
    public void Log_WithIf_WithParams()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(LogMessageWithParams, 69, 96);
        }
    }

    [Benchmark]
    public void LogAdapter_WithParams()
    {
        _loggerAdapter.LogInformation(LogMessageWithParams, 69, 96);
    }

    [Benchmark]
    public void Serilog_WithParams()
    {
        _serilog.Information(LogMessageWithParams, 69, 96);
    }
}
