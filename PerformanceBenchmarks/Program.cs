using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using PerformanceBenchmarks;

var config = DefaultConfig.Instance
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .AddJob(Job.Default.WithId("Release"));

BenchmarkRunner.Run<ViewModelBenchmarks>(config);
