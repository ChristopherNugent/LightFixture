using Shouldly;

namespace LightFixture.Tests;

[DataFactory(typeof(SomeGeneric<>))]
[DataFactory(typeof(SomeGeneric<,>))]
public partial class OpenGenericRegistrationTest
{
    [Fact]
    public void ShouldResolveOpenGeneric()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();

        var result = provider.Resolve<SomeGeneric<string>>();
        
        result.IsResolved.ShouldBeTrue();
        result.Value.Value.ShouldNotBeNull();

        var result2 = provider.Resolve<SomeGeneric<string, SomeGeneric<string>>>();
        
        result2.IsResolved.ShouldBeTrue();
        result2.Value.Left.ShouldNotBeNull();
        result2.Value.Right.ShouldNotBeNull();
    }

    private sealed class SomeGeneric<T>
    {
        public T?  Value { get; set; }

        public int NotGeneric { get; set; }
    }

    private sealed record SomeGeneric<TCheese, TSandwich>(TCheese Left, TSandwich Right);
}