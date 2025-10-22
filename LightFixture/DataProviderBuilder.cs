using LightFixture.Providers;

namespace LightFixture;

public sealed class DataProviderBuilder
{
    private readonly Dictionary<Type, Func<DataProvider, CreationRequest, ResolvedData<object>>> _factories = new();
    private readonly List<Func<DataProvider, CreationRequest, ResolvedData<object>>> _fallbackFactories = new();
    private readonly List<Action<DataProvider, object>> _postProcessors = new();
    private readonly Dictionary<Type, List<object>> _typedPostProcessors = new();

    public DataProviderBuilder()
    {
        Customize(new NumericProvider());
        Customize(StringProvider.Instance);
        Customize(GuidProvider.Instance);
        Customize(CollectionProvider.Instance);
        Customize(new DictionaryProvider());
        Customize(new EnumProvider());
        Customize(new BooleanProvider());
        Customize(new DateTimeProvider());
    }

    public DataProviderBuilder Register<T>(
        Func<DataProvider, CreationRequest, ResolvedData<T>> factory,
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
        Func<DataProvider, CreationRequest, ResolvedData<object>> factory,
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

    public DataProviderBuilder AddPostProcessor<T>(Action<DataProvider, T> postProcessor)
    {
        var key = typeof(T);
        if (!_typedPostProcessors.TryGetValue(key, out var actions))
        {
            actions = [];
            _typedPostProcessors[key] = actions;
        }
        
        actions.Add(postProcessor);

        return this;
    }

    public DataProviderBuilder AddPostProcessor(Action<DataProvider, object> postProcessor)
    {
        _postProcessors.Add(postProcessor);
        return this;
    }

    public DataProvider Build(bool errorIfNoFactory = false) => new(
        _factories,
        _fallbackFactories,
        _postProcessors,
        _typedPostProcessors,
        errorIfNoFactory);
}