namespace LightFixture;

public sealed class DataProvider
{
    private readonly Dictionary<Type, object> _factories;

    private readonly HashSet<Type> _typeStack = [];
    
    internal DataProvider(Dictionary<Type, object> factories)
    {
        _factories = factories;
    }

    public ResolvedData<T> Resolve<T>(CreationRequest? creationRequest = null)
    {
        creationRequest = new(typeof(T), creationRequest?.PropertyName);
        return Resolve(creationRequest.Value).AsGeneric<T>();
    }

    public ResolvedData<object> Resolve(CreationRequest creationRequest)
    {
        if (creationRequest.RequestedType is not { } resolvedType)
        {
            return ResolvedData<object>.NoData;
        }
        
        if (!_typeStack.Add(resolvedType))
        {
            return default;
        }

        var factory = GetFactory(resolvedType);
        var createdObject = factory switch
        {
            Func<DataProvider, CreationRequest?, ResolvedData<object>> f => f(
                this,
                creationRequest),
            _ => throw new InvalidOperationException($"Unknown factory type: {factory.GetType().FullName}")
        };
        
        _typeStack.Remove(resolvedType);
        return createdObject;
    }
    
    internal IEnumerable<T> GetMany<T>(CreationRequest? creationRequest = null, int count = 3)
    {
        var request = new CreationRequest(typeof(T), creationRequest?.PropertyName);
        for (var i = 0; i < count; i++)
        {
            var result = Resolve(request);
            if (!result.IsResolved)
            {
                yield break;
            }
            yield return (T) result.Value;
        }
    }

    internal IEnumerable<object> GetMany(CreationRequest creationRequest, int count = 3)
    {
        for (var i = 0; i < count; i++)
        {
            var result = Resolve(creationRequest);
            if (!result.IsResolved)
            {
                yield break;
            }
            yield return result.Value;
        }
    }

    private object GetFactory(Type typeToResolve)
    {
        if (_factories.TryGetValue(typeToResolve, out var factory))
        {
            return factory;
        }

        if (_factories.TryGetValue(typeToResolve.GetGenericTypeDefinition(), out factory))
        {
            return factory;
        }
        
        throw new KeyNotFoundException($"Unknown factory type: {typeToResolve.FullName}");
    }
}