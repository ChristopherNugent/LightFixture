using Shouldly;

namespace LightFixture.Tests;

[DataFactory(typeof(SomeData))]
[DataFactoryObsoleteHandling(ObsoleteHandlingBehavior.SuppressWarnings)]
public sealed partial class SuppressObsoleteWarningsTests
{
    [Fact]
    public void ShouldSuppressErrors()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();
        
#pragma warning disable CS0612 // Type or member is obsolete
        provider.Resolve<SomeData>().Value.Text.ShouldNotBeNull();
#pragma warning restore CS0612 // Type or member is obsolete
    }
    
    public sealed class SomeData
    {
        [Obsolete]
        public string? Text { get; set; }
    }    
}