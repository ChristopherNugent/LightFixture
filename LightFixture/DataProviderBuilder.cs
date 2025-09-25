using LightFixture.Providers;

namespace LightFixture;

public sealed class DataProviderBuilder
{
    private readonly Dictionary<Type, object> _factories = new();

    public DataProviderBuilder()
    {
        Customize(NumericProvider.Instance);
        Customize(StringProvider.Instance);
        Customize(GuidProvider.Instance);
        Customize(CollectionProvider.Instance);
    }
    
    public DataProviderBuilder Register<T>(Func<DataProvider, CreationRequest?, ResolvedData<T>> factory)
    {
        var type = typeof(T);
        return Register(
            type,
            (p, c) => factory(p, c).AsNonGeneric());
    }

    public DataProviderBuilder Register(
        Type type,
        Func<DataProvider, CreationRequest?, ResolvedData<object>> factory)
    {
        _factories[type] = factory;
        return this;
    }

    public DataProviderBuilder Customize(IDataProviderCustomization customization)
    {
        customization.Apply(this);
        return this;
    }

    public DataProvider Build() => new(_factories);
}
