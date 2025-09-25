using System.Collections.Generic;

namespace LightFixture.Benchmark;

[DataFactory]
public partial class SampleDataProvider
{
    public partial SampleDataOuter SomeData();
}

public class SampleDataOuter
{
    public string? SomeString { get; set; }
    
    public SampleDataInner? SomeInner { get; set; }
    
    // public SampleDataOuter? Recursion { get; set; }
    
    public List<SampleDataInner>? InnerList { get; set; }
    
    public HashSet<string>? InnerHashSet { get; set; }
    
    public Queue<SampleDataInner>? InnerQueue { get; set; }
    
    public Stack<SampleDataInner>? InnerStack { get; set; }
    
    public Dictionary<string, int>?  Dictionary { get; set; }
    
    public IReadOnlyDictionary<double, string>? ReadOnlyDictionary { get; set; }
    
    public IDictionary<SampleDataInner, SampleDataInner>? ComplexDictionary { get; set; }
    
    public ConstructorType? ConstructorType { get; set; }
}

public class SampleDataInner
{
    public int Int { get; set; }
    
    public double? Double { get; set; }
    
    public float? Float { get; set; }
    
    public long? Long { get; set; }
    
    public decimal Decimal { get; set; }

    public string? InnerString { get; set; }
}

public sealed class ConstructorType
{
    public ConstructorType(int someInt)
    {
        SomeInt = someInt;
    }
    
    public int SomeInt { get; set; }

    public string OtherProperty { get; set; } = "";
}