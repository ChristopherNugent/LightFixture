using Shouldly;

namespace LightFixture.Tests;

public sealed class DataProviderTests
{
    [Fact]
    public void ResolveNumerics()
    {
        var provider = GetDefaultProvider();
        
        provider.Resolve<int>().Value.ShouldBe(1);
        provider.Resolve<int?>().Value.ShouldBe(2);
        
        provider.Resolve<long?>().Value.ShouldBe(3);
        provider.Resolve<long>().Value.ShouldBe(4);
        
        provider.Resolve<float>().Value.ShouldBe(5);
        provider.Resolve<float?>().Value.ShouldBe(6);
        
        provider.Resolve<double>().Value.ShouldBe(7);
        provider.Resolve<double?>().Value.ShouldBe(8);
        
        provider.Resolve<decimal>().Value.ShouldBe(9);
        provider.Resolve<decimal?>().Value.ShouldBe(10);
    }

    [Fact]
    public void ResolveStrings()
    {
        var provider = GetDefaultProvider();
        
        Guid.TryParse(provider.Resolve<string>(), out _).ShouldBeTrue();
        provider.Resolve<string>(new CreationRequest(typeof(string),"PropertyName")).Value
            .Split("_", 2)
            .ShouldSatisfyAllConditions(
                x => x[0].ShouldBe("PropertyName"),
                x => Guid.TryParse(x[1], out _).ShouldBeTrue());
    }
    
    private static DataProvider GetDefaultProvider() => new DataProviderBuilder().Build();
}