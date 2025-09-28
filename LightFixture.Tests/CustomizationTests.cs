using Shouldly;

namespace LightFixture.Tests;

[DataFactory]
public sealed partial class CustomizationTests
{
    [Fact]
    public void BasicCustomization()
    {
        const int testValue = 1234567889;

        var provider = new DataProviderBuilder()
            .Customize(this)
            .AddPostProcessor<BasicType>((_, bt) => bt.Value = testValue)
            .Build();
        
        provider.ResolveMany<BasicType>(count: 10).ShouldAllBe(x => x.Value == testValue);
    }

    [DataFactory]
    private partial BasicType GetData();

    private sealed class BasicType
    {
        public int Value { get; set; }
    }
}