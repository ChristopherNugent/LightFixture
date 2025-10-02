using System;
using System.Collections.Generic;

namespace LightFixture.SourceGeneration.Sample;

[DataFactory(typeof(Inherited))]
public partial class SampleDataProvider
{
}

public class Base
{
    public int A { get; set; }
    
    public int B { get => default; set => throw new Exception("This should never be set"); }
}

public sealed class Inherited : Base
{
    public new int B { get; set; }
}