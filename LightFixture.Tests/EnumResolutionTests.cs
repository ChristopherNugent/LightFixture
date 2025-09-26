using Shouldly;

namespace LightFixture.Tests;

public sealed class EnumResolutionTests
{
    [Fact]
    public void Tests()
    {
        var provider = new DataProviderBuilder().Build();
        
        provider.ResolveMany<TestEnum>(count: 4)
            .ShouldBe([TestEnum.A, TestEnum.B, TestEnum.A, TestEnum.B]);
    }

    private enum TestEnum
    {
        A,
        B,
    }
}