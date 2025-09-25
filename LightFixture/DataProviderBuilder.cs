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
        Customize(DictionaryProvider.Instance);
    }
    
    public DataProviderBuilder Register<T>(
        Func<DataProvider, CreationRequest?, ResolvedData<T>> factory,
        bool overrideExisting = false)
    {
        var type = typeof(T);
        return Register(
            type,
            (p, c) => factory(p, c).AsNonGeneric(),
            overrideExisting);
    }

    public DataProviderBuilder Register(
        Type type,
        Func<DataProvider, CreationRequest?, ResolvedData<object>> factory,
        bool overrideExisting = false)
    {
        if (overrideExisting || !_factories.ContainsKey(type))
        {
            _factories[type] = factory;
        }
        return this;
    }

    public DataProviderBuilder Customize(IDataProviderCustomization customization)
    {
        customization.Apply(this);
        return this;
    }

    public DataProvider Build() => new(_factories);
}
