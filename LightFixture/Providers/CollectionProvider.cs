using System.Collections;
using System.Collections.Concurrent;

namespace LightFixture.Providers;

internal sealed class CollectionProvider : IDataProviderCustomization
{
    public static readonly CollectionProvider Instance = new();
    
    private readonly ConcurrentDictionary<Type, IEnumerableProvider> _providers = new ();

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
    }

    private sealed class TypedEnumerableProvider<T> : IEnumerableProvider
    {
        private IEnumerable<T> GetItems(DataProvider provider, CreationRequest request) => provider.ResolveMany<T>(request);

        public object Get(DataProvider provider, CreationRequest request) => GetItems(provider, request);
    }

    private interface IEnumerableProvider
    {
        object Get(DataProvider provider, CreationRequest request);
    }
}