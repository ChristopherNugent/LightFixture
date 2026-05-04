using Shouldly;

namespace LightFixture.Tests;

[DataFactory(typeof(SomeData))]
[DataFactoryObsoleteHandling(ObsoleteHandlingBehavior.SuppressErrors)]
public sealed partial class SuppressObsoleteErrorsTests
{
    [Fact]
    [Obsolete]
    public void ShouldSuppressWarnings()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();
        
        provider.Resolve<SomeData>().Value.Text.ShouldNotBeNull();
    }
    
    public sealed class SomeData
    {
        [Obsolete("", error:true)]
        public string? Text { get; set; }
    }    
}