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

        var data = provider.Resolve<TestType>();
    }

    private sealed class TestType
    {
        public int IgnoredProperty
        {
            get => -1;
            set => Assert.Fail("This setter should never be accessed.");
        }
    }
}