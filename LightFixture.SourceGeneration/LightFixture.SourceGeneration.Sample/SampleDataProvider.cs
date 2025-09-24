namespace LightFixture.SourceGeneration.Sample;

[DataFactory]
public partial class SampleDataProvider
{
    public partial SampleDataOuter SomeData();
}

public class SampleDataOuter
{
    public string? SomeString { get; set; }
    
    public SampleDataInner? SomeInner { get; set; }
    
    public SampleDataOuter? Recursion { get; set; }
}

public class SampleDataInner
{
    public int Int { get; set; }
    
    public double? Double { get; set; }
    
    public float? Float { get; set; }
    
    public long? Long { get; set; }
    
    public decimal Decimal { get; set; }
}