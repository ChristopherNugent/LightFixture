using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace LightFixture.Benchmark;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ObjectCreationBenchmark
{
    private static readonly Fixture Fixture = new();

    private static readonly DataProvider DataProvider = new DataProviderBuilder()
        .Customize(new DataFactory())
        .Build();

    [Benchmark(Baseline = true), BenchmarkCategory("WideOfWide")]
    public WideContainer<WideContainer<int>> WideOfWide_LightFixture()
        => DataProvider.Resolve<WideContainer<WideContainer<int>>>();

    [Benchmark, BenchmarkCategory("WideOfWide")]
    public WideContainer<WideContainer<int>> WideOfWide_AutoFixture()
        => Fixture.Create<WideContainer<WideContainer<int>>>();
    
    [Benchmark(Baseline = true), BenchmarkCategory("WideOfNarrow")]
    public WideContainer<NarrowContainer<int>> WideOfNarrow_LightFixture()
        => DataProvider.Resolve<WideContainer<NarrowContainer<int>>>();
    
    [Benchmark, BenchmarkCategory("WideOfNarrow")]
    public WideContainer<NarrowContainer<int>> WideOfNarrow_AutoFixture()
        => Fixture.Create<WideContainer<NarrowContainer<int>>>();
    
    [Benchmark(Baseline = true), BenchmarkCategory("NarrowOfNarrow")]
    public NarrowContainer<NarrowContainer<int>> NarrowOfNarrow_LightFixture()
        => DataProvider.Resolve<NarrowContainer<NarrowContainer<int>>>();
    
    [Benchmark, BenchmarkCategory("NarrowOfNarrow")]
    public NarrowContainer<NarrowContainer<int>> NarrowOfNarrow_AutoFixture()
        => Fixture.Create<NarrowContainer<NarrowContainer<int>>>();
    
    [Benchmark(Baseline = true), BenchmarkCategory("NarrowOfWide")]
    public NarrowContainer<WideContainer<int>> NarrowOfWide_LightFixture()
        => DataProvider.Resolve<NarrowContainer<WideContainer<int>>>();
    
    [Benchmark, BenchmarkCategory("NarrowOfWide")]
    public NarrowContainer<WideContainer<int>> NarrowOfWide_AutoFixture()
        => Fixture.Create<NarrowContainer<WideContainer<int>>>();
    
    [Benchmark(Baseline = true), BenchmarkCategory("StructNarrowOfNarrow")]
    public StructNarrowContainer<StructNarrowContainer<int>> NarrowStruct_LightFixture()
        => DataProvider.Resolve<StructNarrowContainer<StructNarrowContainer<int>>>();
    
    [Benchmark, BenchmarkCategory("StructNarrowOfNarrow")]
    public StructNarrowContainer<StructNarrowContainer<int>> NarrowStruct_AutoFixture()
        => Fixture.Create<StructNarrowContainer<StructNarrowContainer<int>>>();
}