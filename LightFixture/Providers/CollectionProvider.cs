using System.Collections;

namespace LightFixture.Providers;

internal sealed class CollectionProvider : IDataProviderCustomization
{
    public static readonly CollectionProvider Instance = new CollectionProvider();

    private CollectionProvider()
    {
    }

    private static ResolvedData<object> MakeIList(DataProvider p, CreationRequest? creationRequest = null)
    {
        if (creationRequest?.RequestedType is not { GenericTypeArguments.Length: 1 } listType)
        {
            return ResolvedData<object>.NoData;
        }

        var elementType = listType.GenericTypeArguments[0];
        var enumerable = p.GetMany(creationRequest.Value with { RequestedType = elementType });
        
        var list = (IList) Activator.CreateInstance(creationRequest.Value.RequestedType)!;

        foreach (var o in enumerable)
        {
            list.Add(o);
        }

        return (object) list;
    }
    
    public void Apply(DataProviderBuilder builder)
    {
        builder.RegisterInternal(typeof(List<>), static (p, r) => MakeIList(p, r));
    }
}