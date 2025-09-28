using Shouldly;

namespace LightFixture.Tests;

[DataFactory]
public sealed partial class BasicTypeResolutionTest
{
    [Fact]
    public void BasicTypeResolution()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();

        var data = provider.Resolve<TestType>();

        data.Value
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                d => d.SomeDouble.ShouldBe(2),
                d => d.Inner.ShouldNotBeNull()
                    .ShouldSatisfyAllConditions(
                        d2 => d2.SomeInt.ShouldBe(1),
                        d2 => d2.SomeString.ShouldNotBeNull().Length.ShouldBe(47),
                        d2 => d2.Recursion.ShouldBeNull()));
    }

    [DataFactory]
    private partial TestType MakeData();
    
    public sealed class TestType
    {
        public InnerType? Inner { get; set; }
        
        public double SomeDouble { get; set; }
    }

    public sealed class InnerType
    {
        public string? SomeString { get; set; }
        
        public int SomeInt { get; set; }

        public TestType? Recursion { get; set; }
    }
}
