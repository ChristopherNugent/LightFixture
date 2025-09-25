using System.Collections.Generic;

namespace LightFixture.SourceGeneration.Sample;

[DataFactory]
public partial class SampleDataProvider
{
    public partial SampleData SomeData();
}

public sealed class SampleData
{
    public int Int { get; set; }
    
    public double? Double { get; set; }
}
