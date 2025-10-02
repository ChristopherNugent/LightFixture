namespace LightFixture.SourceGeneration.Sample;

[DataFactory(typeof(T1))]
public partial class SampleDataProvider
{
}

public sealed class T1
{
    public T2[] Array { get; set; } = [];
}

public sealed class T2
{
    public int Value { get; set; }
}