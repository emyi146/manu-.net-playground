// You are doing .NET logging wrong. Let's fix it
// https://www.youtube.com/watch?v=bnVfrd3lRv8
// Check Benchmarks.cs for more details

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Logging.MemoryConsiderations;

// Benchmarks:

var config = new ManualConfig()
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .AddLogger(ConsoleLogger.Default)
    .AddColumnProvider(DefaultColumnProviders.Instance);
BenchmarkRunner.Run<Benchmarks>(config);