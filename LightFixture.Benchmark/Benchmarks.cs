using AutoFixture;
using BenchmarkDotNet.Attributes;

namespace LightFixture.Benchmark;

[MemoryDiagnoser]
public class Benchmarks
{
    private static readonly DataProvider Provider = new DataProviderBuilder()
        .Customize(new SampleDataProvider())
        .Build();
    
    private static readonly Fixture Fixture = new Fixture();
    
    [Benchmark]
    public SampleDataOuter LightFixture()
    {
        return Provider.Resolve<SampleDataOuter>();
    }

    [Benchmark]
    public SampleDataOuter AutoFixture()
    {
        return Fixture.Create<SampleDataOuter>();
    }
}

