using System.Collections;
using System.Collections.Concurrent;

namespace LightFixture.Providers;

internal sealed class CollectionProvider : IDataProviderCustomization
{
    public static readonly CollectionProvider Instance = new();
    
    private readonly ConcurrentDictionary<Type, IEnumerableProvider> _providers = new ();
    private readonly ConcurrentDictionary<Type, IArrayProvider> _arrayProviders = new ();

    private CollectionProvider()
    {
    }

    private ResolvedData<object> MakeEnumerableAcceptor(DataProvider provider, CreationRequest creationRequest)
    {
        if (creationRequest.RequestedType?.GenericTypeArguments is not [var elementType])
        {
            return ResolvedData<object>.NoData;
        }

        if(!_providers.TryGetValue(elementType, out var seriesProvider))
        {
            seriesProvider = (IEnumerableProvider)Activator.CreateInstance(
                typeof(TypedEnumerableProvider<>).MakeGenericType(elementType))!;
            
            _providers.TryAdd(elementType, seriesProvider);
        }
        
        return Activator.CreateInstance(
            creationRequest.RequestedType,
            seriesProvider.Get(provider, creationRequest))!;
    }

    public ResolvedData<object> MakeArray(DataProvider provider, CreationRequest creationRequest)
    {
        if (creationRequest.RequestedType?.IsArray is not true
            || creationRequest.RequestedType.GetElementType() is not {} elementType)
        {
            return ResolvedData<object>.NoData;
        }
        
        if(!_arrayProviders.TryGetValue(elementType, out var seriesProvider))
        {
            seriesProvider = (IArrayProvider)Activator.CreateInstance(
                typeof(TypedArrayProvider<>).MakeGenericType(elementType))!;
            
            _arrayProviders.TryAdd(elementType, seriesProvider);
        }

        return seriesProvider.Get(provider, creationRequest);
    }
    
    public void Apply(DataProviderBuilder builder)
    {
        builder.Register(typeof(List<>), MakeEnumerableAcceptor);
        builder.Register(typeof(IList<>), (p, r) => MakeEnumerableAcceptor(p, r with
        {
            RequestedType = typeof(List<>).MakeGenericType(r.RequestedType!.GenericTypeArguments[0])
        }));
        builder.Register(typeof(IReadOnlyList<>), (p, r) => MakeEnumerableAcceptor(p, r with
        {
            RequestedType = typeof(List<>).MakeGenericType(r.RequestedType!.GenericTypeArguments[0])
        }));
        builder.Register(typeof(IEnumerable<>), (p, r) => MakeEnumerableAcceptor(p, r with
        {
            RequestedType = typeof(List<>).MakeGenericType(r.RequestedType!.GenericTypeArguments[0])
        }));
        builder.Register(typeof(ICollection<>), (p, r) => MakeEnumerableAcceptor(p, r with
        {
            RequestedType = typeof(List<>).MakeGenericType(r.RequestedType!.GenericTypeArguments[0])
        }));
        builder.Register(typeof(IReadOnlyCollection<>), (p, r) => MakeEnumerableAcceptor(p, r with
        {
            RequestedType = typeof(List<>).MakeGenericType(r.RequestedType!.GenericTypeArguments[0])
        }));
            
        builder.Register(typeof(HashSet<>), MakeEnumerableAcceptor);
        builder.Register(typeof(ISet<>), (p, r) => MakeEnumerableAcceptor(p, r with
        {
            RequestedType = typeof(HashSet<>).MakeGenericType(r.RequestedType!.GenericTypeArguments[0])
        }));
        builder.Register(typeof(IReadOnlySet<>), (p, r) => MakeEnumerableAcceptor(p, r with
        {
            RequestedType = typeof(HashSet<>).MakeGenericType(r.RequestedType!.GenericTypeArguments[0])
        }));
        
        builder.Register(typeof(Queue<>), MakeEnumerableAcceptor);
        builder.Register(typeof(Stack<>), MakeEnumerableAcceptor);
        builder.Register(MakeArray);
    }

    private sealed class TypedEnumerableProvider<T> : IEnumerableProvider
    {
        private IEnumerable<T> GetItems(DataProvider provider, CreationRequest request) => provider.ResolveMany<T>(request);

        public IEnumerable Get(DataProvider provider, CreationRequest request) => GetItems(provider, request);
    }
    
    private sealed class TypedArrayProvider<T> : IArrayProvider
    {
        private T[] GetItems(DataProvider provider, CreationRequest request)
            => provider.ResolveMany<T>(request).ToArray();

        public object Get(DataProvider provider, CreationRequest request) => GetItems(provider, request);
    }

    private interface IArrayProvider
    {
        object Get(DataProvider provider, CreationRequest request);
    }
    
    private interface IEnumerableProvider
    {
        IEnumerable Get(DataProvider provider, CreationRequest request);
    }
}