namespace LightFixture.Benchmark;

[DataFactory(typeof(WideContainer<NarrowContainer<int>>))]
[DataFactory(typeof(WideContainer<WideContainer<int>>))]
[DataFactory(typeof(NarrowContainer<WideContainer<int>>))]
[DataFactory(typeof(NarrowContainer<NarrowContainer<int>>))]
[DataFactory(typeof(StructNarrowContainer<StructNarrowContainer<int>>))]
[DataFactory(typeof(WideContainer<int>))]
[DataFactory(typeof(NarrowContainer<int>))]
[DataFactory(typeof(StructNarrowContainer<int>))]
public sealed partial class DataFactory;