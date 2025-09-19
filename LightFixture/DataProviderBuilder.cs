namespace LightFixture;

public sealed class DataProviderBuilder
{
    private readonly Dictionary<Type, object> _factories = new();
    
    public DataProviderBuilder Register<T>(Func<DataProvider, CreationRequest?, ResolvedData<T>> factory)
    {
        var type = typeof(T);
        _factories[type] = factory;
        if (type.IsValueType)
        {
            _factories[typeof(Nullable<>).MakeGenericType(type)] = factory;
        }
        return this;
    }
    
    public DataProviderBuilder Register<T>(Func<DataProvider, ResolvedData<T>> factory)
        => Register((p, _) => factory(p));
    
    public DataProviderBuilder Register<T>(Func<ResolvedData<T>> factory)
        => Register((_, _) => factory());

    public DataProviderBuilder With(IDataProviderCustomization customization)
    {
        customization.Apply(this);
        return this;
    }

    public DataProvider Build() => new(_factories);
}