using LightFixture.Providers;

namespace LightFixture;

public sealed class DataProviderBuilder
{
    private readonly Dictionary<Type, Func<DataProvider, CreationRequest?, ResolvedData<object>>> _factories = new();
    private readonly List<Func<DataProvider, CreationRequest, ResolvedData<object>>> _fallbackFactories = new();

    public DataProviderBuilder()
    {
        Customize(new NumericProvider());
        Customize(StringProvider.Instance);
        Customize(GuidProvider.Instance);
        Customize(CollectionProvider.Instance);
        Customize(new DictionaryProvider());
        Customize(new EnumProvider());
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
    
    public DataProviderBuilder Register(Func<DataProvider, CreationRequest, ResolvedData<object>> factory)
    {
       _fallbackFactories.Add(factory);
        return this;
    }

    public DataProviderBuilder Customize(IDataProviderCustomization customization)
    {
        customization.Apply(this);
        return this;
    }

    public DataProvider Build() => new(_factories, _fallbackFactories);
}
