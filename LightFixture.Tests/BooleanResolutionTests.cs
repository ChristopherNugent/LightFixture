using Shouldly;

namespace LightFixture.Tests;

public sealed class BooleanResolutionTests
{
    [Fact]
    public void BooleanResolution()
    {
        var builder = new DataProviderBuilder().Build();

        // Seed should be fixed.
        bool[] expected = [true, false, false, true, false, false, false, true, false, true];
        builder.ResolveMany<bool>(count: 10).ShouldBe(expected);
    }
}