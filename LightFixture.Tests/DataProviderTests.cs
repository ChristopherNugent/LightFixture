using Shouldly;

namespace LightFixture.Tests;

public sealed class DataProviderTests
{
    [Fact]
    public void ResolveInt()
    {
        var provider = GetDefaultProvider();
        
        provider.Resolve<int>().Value.ShouldBe(1);
        provider.Resolve<int?>().Value.ShouldBe(1);
    }
    
    private static DataProvider GetDefaultProvider() => new DataProviderBuilder().Build();
}