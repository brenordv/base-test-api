using BenchmarkDotNet.Running;
using Raccoon.Ninja.Application.Benchmarks.AutoMapper;

Console.WriteLine("Running AutoMapper Benchmarks!");
BenchmarkRunner.Run<AutoMapperBenchmarks>();