namespace LightFixture.Tests;

[DataFactory(typeof(TestType))]
[DataFactoryIgnoreProperty(typeof(TestType), nameof(TestType.IgnoredProperty))]
public sealed partial class IgnoredPropertyTests
{
    [Fact]
    public void IgnoredProperty()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();

        provider.Resolve<TestType>();
    }

    private sealed class TestType
    {
        public TestType2 IgnoredProperty
        {
            get => new();
            [Obsolete("Break the source gen if referenced", true)]
            set => Assert.Fail("This setter should never be accessed.");
        }
    }

    private sealed class TestType2;
}