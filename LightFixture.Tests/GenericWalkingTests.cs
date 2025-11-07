using Shouldly;

namespace LightFixture.Tests;

[DataFactory(typeof(Wrapper))]
public sealed partial class GenericWalkingTests
{
    [Fact]
    public void ShouldWalkTypeArguments()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();
        
        provider.Resolve<Wrapper>().Value.SomeList.ShouldNotBeEmpty();
    }
    
    private sealed class Wrapper
    {
        public List<Wrapped>? SomeList { get; set; }
    }

    private sealed class Wrapped;
}