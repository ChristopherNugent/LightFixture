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
            return default;
        }

        var factory = (Func<DataProvider, CreationRequest?, ResolvedData<T>>) _factories[resolvedType];
        var createdObject = factory(this, creationRequest);
        _typeStack.Remove(resolvedType);
        return createdObject;
    }
}