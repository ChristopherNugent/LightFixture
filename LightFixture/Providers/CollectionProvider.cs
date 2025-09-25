using System.Collections;
using System.Collections.Concurrent;

namespace LightFixture.Providers;

internal sealed class CollectionProvider : IDataProviderCustomization
{
    public static readonly CollectionProvider Instance = new CollectionProvider();
    
    private readonly ConcurrentDictionary<Type, IEnumerableProvider> _providers = new ();

    private CollectionProvider()
    {
    }

    private ResolvedData<object> MakeEnumerableAcceptor(DataProvider provider, CreationRequest? creationRequest = null)
    {
        if (creationRequest?.RequestedType is not { GenericTypeArguments.Length: 1 } listType)
        {
            return ResolvedData<object>.NoData;
        }

        var elementType = listType.GenericTypeArguments[0];
        if(!_providers.TryGetValue(elementType, out var seriesProvider))
        {
            seriesProvider = (IEnumerableProvider)Activator.CreateInstance(
                typeof(TypedEnumerableProvider<>).MakeGenericType(elementType))!;
            
            _providers.TryAdd(elementType, seriesProvider);
        }
        
        return Activator.CreateInstance(
            creationRequest.Value.RequestedType,
            seriesProvider.Get(provider, creationRequest.Value))!;
    }
    
    public void Apply(DataProviderBuilder builder)
    {
        builder.RegisterInternal(typeof(List<>), MakeEnumerableAcceptor);
        builder.RegisterInternal(typeof(HashSet<>), MakeEnumerableAcceptor);
        builder.RegisterInternal(typeof(Queue<>), MakeEnumerableAcceptor);
        builder.RegisterInternal(typeof(Stack<>), MakeEnumerableAcceptor);
    }

    private sealed class TypedEnumerableProvider<T> : IEnumerableProvider
    {
        private IEnumerable<T> GetItems(DataProvider provider, CreationRequest request) => provider.GetMany<T>(request);

        public object Get(DataProvider provider, CreationRequest request) => GetItems(provider, request);
    }

    private interface IEnumerableProvider
    {
        object Get(DataProvider provider, CreationRequest request);
    }
}