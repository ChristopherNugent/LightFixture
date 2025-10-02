using Shouldly;

namespace LightFixture.Tests;

[DataFactory(typeof(Inherited))]
public sealed partial class InheritedTypeResolutionTests
{
    [Fact]
    public void ShouldSetValuesFromBaseTypes()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();
        
        provider.Resolve<Inherited>().Value.ShouldSatisfyAllConditions(
            x => x.A.ShouldBe(2),
            x => x.B.ShouldBe(1));
    }
    
    public class Base
    {
        public int A { get; set; }
    }

    public sealed class Inherited : Base
    {
        public int B { get; set; }
    }
}