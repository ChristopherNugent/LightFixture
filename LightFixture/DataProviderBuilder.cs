namespace LightFixture;

public sealed class DataProviderBuilder
{
    private readonly Dictionary<Type, object> _factories = new();
    
    public DataProviderBuilder Register<T>(Func<DataProvider, CreationRequest?, ResolvedData<T>> factory)
    {
        var type = typeof(T);
        _factories[type] = factory;
        return this;
    }

    public DataProviderBuilder With(IDataProviderCustomization customization)
    {
        customization.Apply(this);
        return this;
    }

    public DataProvider Build() => new(_factories);
}