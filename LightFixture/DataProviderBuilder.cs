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
    }
    
    internal DataProviderBuilder RegisterInternal<T>(Func<DataProvider, CreationRequest?, ResolvedData<T>> factory)
    {
        var type = typeof(T);
        _factories[type] = factory;
        return this;
    }

    internal DataProviderBuilder RegisterInternal(Type type,
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

public static class DataProviderBuilderClassExtensions
{
    public static DataProviderBuilder Register<T>(
        this DataProviderBuilder builder,
        Func<DataProvider, CreationRequest?, ResolvedData<T>> factory)
        where T : class
        => builder.RegisterInternal<T>(factory);
    
    public static DataProviderBuilder Register<T>(
        this DataProviderBuilder builder,
        Func<DataProvider, ResolvedData<T>> factory)
        where T : class
        => builder.Register((p, _) => factory(p));
    
    public static DataProviderBuilder Register<T>(
        this DataProviderBuilder builder,
        Func<ResolvedData<T>> factory)
        where T : class
        => builder.Register((_, _) => factory());
    
    public static DataProviderBuilder Register(
        this DataProviderBuilder builder,
        Func<DataProvider, CreationRequest?, ResolvedData<object>> factory)
        => builder.RegisterInternal(factory);
    
    public static DataProviderBuilder Register(
        this DataProviderBuilder builder,
        Func<DataProvider, ResolvedData<object>> factory)
        => builder.Register((p, _) => factory(p));
    
    public static DataProviderBuilder Register(
        this DataProviderBuilder builder,
        Func<ResolvedData<object>> factory)
        => builder.Register((_, _) => factory());
}

public static class DataProviderBuilderStructExtensions
{
    public static DataProviderBuilder Register<T>(
        this DataProviderBuilder builder,
        Func<DataProvider, CreationRequest?, ResolvedData<T>> factory)
        where T : struct
    {
        builder.RegisterInternal(factory);
        builder.RegisterInternal<T?>((p, r) =>
        {
            var result = factory(p, r);
            return result.IsResolved 
                ? result.Value 
                : ResolvedData<T?>.NoData;
        });
        return builder;
    }

    public static DataProviderBuilder Register<T>(
        this DataProviderBuilder builder,
        Func<DataProvider, ResolvedData<T>> factory)
        where T : struct
        => builder.Register((p, _) => factory(p));
    
    public static DataProviderBuilder Register<T>(
        this DataProviderBuilder builder,
        Func<ResolvedData<T>> factory)
        where T : struct
        => builder.Register((_, _) => factory());
}