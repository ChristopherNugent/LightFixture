using Shouldly;

namespace LightFixture.Tests;

[DataFactory(typeof(SomeData))]
[DataFactoryObsoleteHandling(ObsoleteHandlingBehavior.IgnoreObsolete)]
public sealed partial class IgnoreObsoleteTests
{
    [Fact]
    public void ShouldIgnoreObsolete()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();
        
#pragma warning disable CS0612 // Type or member is obsolete
        provider.Resolve<SomeData>().Value.Text.ShouldBeNull();
#pragma warning restore CS0612 // Type or member is obsolete
    }
    
    public sealed class SomeData
    {
        [Obsolete]
        public string? Text { get; set; }
    }    
}