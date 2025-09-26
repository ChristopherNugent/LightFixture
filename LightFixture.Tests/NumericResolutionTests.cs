using Shouldly;

namespace LightFixture.Tests;

public sealed class NumericResolutionTests
{
    [Fact]
    public void Bytes()
    {
        var provider = new DataProviderBuilder().Build();

        provider.ResolveMany<byte>().ShouldBe([1, 2, 3]);
    }
    
    [Fact]
    public void Shorts()
    {
        var provider = new DataProviderBuilder().Build();

        provider.ResolveMany<short>().ShouldBe([1, 2, 3]);
    }
    
    [Fact]
    public void Ints()
    {
        var provider = new DataProviderBuilder().Build();

        provider.ResolveMany<int>().ShouldBe([1, 2, 3]);
    }

    [Fact]
    public void Longs()
    {
        var provider = new DataProviderBuilder().Build();
        
        provider.ResolveMany<long>().ShouldBe([1, 2, 3]);
    }

    [Fact]
    public void Floats()
    {
        var provider = new DataProviderBuilder().Build();
        
        provider.ResolveMany<float>().ShouldBe([1, 2, 3]);
    }

    [Fact]
    public void Doubles()
    {
        var provider = new DataProviderBuilder().Build();

        provider.ResolveMany<double>().ShouldBe([1, 2, 3]);
    }

    [Fact]
    public void Decimals()
    {
        var provider = new DataProviderBuilder().Build();

        provider.ResolveMany<decimal>().ShouldBe([1, 2, 3]);
    }
}