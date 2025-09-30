using System.Collections.Concurrent;

namespace LightFixture.Providers;

internal sealed class DictionaryProvider : IDataProviderCustomization
{
    private static readonly ConcurrentDictionary<(Type, Type), IDictionaryProvider> Providers = new ();

    private ResolvedData<object> MakeDictionary(DataProvider provider, CreationRequest request)
    {
        if (request.RequestedType?.GenericTypeArguments is not [var keyType, var valueType])
        {
            return ResolvedData<object>.NoData;
        }
        
        if(!Providers.TryGetValue((keyType, valueType), out var seriesProvider))
        {
            seriesProvider = (IDictionaryProvider)Activator.CreateInstance(
                typeof(TypedDictionaryProvider<,>).MakeGenericType(keyType, valueType))!;
            
            Providers.TryAdd((keyType, valueType), seriesProvider);
        }
        
        return seriesProvider.Get(provider, request);
    }
    
    public void Apply(DataProviderBuilder builder)
    {
        builder
            .Register(typeof(Dictionary<,>), MakeDictionary)
            .Register(typeof(IDictionary<,>), MakeDictionary)
            .Register(typeof(IReadOnlyDictionary<,>), MakeDictionary);
    }

    private interface IDictionaryProvider
    {
        object Get(DataProvider provider, CreationRequest request);
    }

    private sealed class TypedDictionaryProvider<TKey, TValue> : IDictionaryProvider
        where TKey : notnull
    {
        public object Get(DataProvider provider, CreationRequest request)
        {
            var dict = new Dictionary<TKey, TValue>();

            for (int i = 0; i < 3; i++)
            {
                var key = provider.Resolve<TKey>(request);
                var value = provider.Resolve<TValue>(request);
                if (key.IsResolved && value.IsResolved)
                {
                    dict.Add(key, value);
                }
            }

            return dict;
        }
    }
}