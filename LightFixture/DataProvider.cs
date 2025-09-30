namespace LightFixture;

public sealed class DataProvider
{
    private readonly Dictionary<Type, Func<DataProvider, CreationRequest, ResolvedData<object>>> _factories;
    private readonly List<Func<DataProvider, CreationRequest, ResolvedData<object>>> _fallbackFactories;
    private readonly List<Action<DataProvider, object>> _postProcessors;

    private readonly HashSet<Type> _typeStack = [];
    
    internal DataProvider(
        Dictionary<Type, Func<DataProvider, CreationRequest, ResolvedData<object>>> factories,
        List<Func<DataProvider, CreationRequest, ResolvedData<object>>> fallbackFactories,
        List<Action<DataProvider, object>> postProcessors)
    {
        _factories = factories;
        _fallbackFactories = fallbackFactories;
        _postProcessors = postProcessors;
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
        var createdObject = factory?.Invoke(this, creationRequest) ?? ResolveFallback(creationRequest);
        if (createdObject.IsResolved)
        {
            foreach (var postProcessor in _postProcessors)
            {
                postProcessor(this, createdObject.Value);
            }
        }
        
        _typeStack.Remove(resolvedType);
        return createdObject;
    }
    
    public IEnumerable<T> ResolveMany<T>(CreationRequest? creationRequest = null, int count = 3)
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

    public IEnumerable<object> ResolveMany(CreationRequest creationRequest, int count = 3)
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

    private Func<DataProvider, CreationRequest, ResolvedData<object>>? GetFactory(Type typeToResolve)
    {
        if (_factories.TryGetValue(typeToResolve, out var factory))
        {
            return factory;
        }

        if (Nullable.GetUnderlyingType(typeToResolve) is {} underlyingType
            && _factories.TryGetValue(underlyingType, out factory))
        {
            return factory;
        }

        if (typeToResolve.IsGenericType && _factories.TryGetValue(typeToResolve.GetGenericTypeDefinition(), out factory))
        {
            return factory;
        }

        return null;
    }

    private ResolvedData<object> ResolveFallback(CreationRequest creationRequest)
    {
        foreach (var factory in _fallbackFactories)
        {
            var result = factory(this, creationRequest);
            if (result.IsResolved)
            {
                return result.Value;
            }
        }

        throw new Exception("No registered factory was available for type.");
    }
}