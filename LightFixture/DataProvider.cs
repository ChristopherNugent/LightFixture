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
        var resolvedType = typeof(T);
        if (!_typeStack.Add(resolvedType))
        {
            return ResolvedData<T>.NoData;
        }
        creationRequest ??= new (typeof(T), null);

        var factory = _factories[resolvedType];
        var createdObject = factory switch
        {
            Func<DataProvider, CreationRequest?, ResolvedData<T>> f => f(this, creationRequest),
            Func<DataProvider, CreationRequest?, ResolvedData<object>> f => ResolvedData.FromValue((T)f(
                this,
                creationRequest)),
            _ => throw new InvalidOperationException($"Unknown factory type: {factory.GetType().FullName}")
        };
        
        _typeStack.Remove(resolvedType);
        return createdObject;
    }

    public ResolvedData<object> Resolve(CreationRequest? creationRequest = null)
    {
        if (creationRequest?.RequestedType is not { } resolvedType)
        {
            return ResolvedData<object>.NoData;
        }
        if (!_typeStack.Add(resolvedType))
        {
            return default;
        }
        creationRequest ??= new (typeof(T), null);

        var factory = _factories[resolvedType];
        var createdObject = factory switch
        {
            Func<DataProvider, CreationRequest?, ResolvedData<object>> f => ResolvedData.FromValue(f(
                this,
                creationRequest)),
            _ => throw new InvalidOperationException($"Unknown factory type: {factory.GetType().FullName}")
        };
        
        _typeStack.Remove(resolvedType);
        return createdObject;
    }
}