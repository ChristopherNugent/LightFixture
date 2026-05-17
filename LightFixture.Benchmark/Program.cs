// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using LightFixture.Benchmark;

BenchmarkRunner.Run<ObjectCreationBenchmark>();

// var x = new ObjectCreationBenchmark().NarrowOfNarrow_LightFixture();
// Console.WriteLine(x);