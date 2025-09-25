using AutoFixture;
using BenchmarkDotNet.Attributes;

namespace LightFixture.Benchmark;

[MemoryDiagnoser]
public class Benchmarks
{
    private static readonly DataProvider Provider = new DataProviderBuilder()
        .Customize(new SampleDataProvider())
        .Build();
    
    private static readonly Fixture Fixture = new();

    [Benchmark]
    public SampleDataOuter LightFixtureComplex()
    {
        return Provider.Resolve<SampleDataOuter>();
    }

    [Benchmark]
    public SampleDataOuter AutoFixtureComplex()
    {
        return Fixture.Create<SampleDataOuter>();
    }
    
    [Benchmark]
    public SampleDataInner LightFixtureSimple()
    {
        return Provider.Resolve<SampleDataInner>();
    }

    [Benchmark]
    public SampleDataInner AutoFixtureSimple()
    {
        return Fixture.Create<SampleDataInner>();
    }
    
}

