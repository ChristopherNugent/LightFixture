namespace LightFixture;

public sealed class ThreadSafeDataProvider : DataProvider
{
    private readonly HashSet<Type> _typeStack = [];
    
    internal ThreadSafeDataProvider(
        Dictionary<Type, Func<DataProvider, CreationRequest, ResolvedData<object>>> factories,
        List<Func<DataProvider, CreationRequest, ResolvedData<object>>> fallbackFactories, 
        List<Action<DataProvider, object>> postProcessors,
        Dictionary<Type, List<object>> typedPostProcessors,
        bool errorIfNoFactory) 
        : base(factories, fallbackFactories, postProcessors, typedPostProcessors, errorIfNoFactory)
    {
        
    }

    public override ResolvedData<object> Resolve(CreationRequest creationRequest)
    {
        var scopedProvider = new DataProvider(this);
        return scopedProvider.Resolve(creationRequest);
    }
}