using System.Collections.Generic;

namespace LightFixture.SourceGeneration.Sample;

[DataFactory(typeof(SampleData))]
public partial class SampleDataProvider
{
}

public sealed class SampleData
{
    public int Int { get; set; }
    
    public double? Double { get; set; }
    
    public SampleEnum Enum { get; set; }
    
    public SampleEnum Enum2 { get; set; }
}

public enum SampleEnum
{
    ValueA,
    ValueB,
    ValueC,
}
