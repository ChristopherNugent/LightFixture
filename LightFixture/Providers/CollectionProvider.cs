using System.Collections;

namespace LightFixture.Providers;

internal sealed class CollectionProvider : IDataProviderCustomization
{
    private const int CollectionCount = 3;
    
    private static object? MakeList(DataProvider p, CreationRequest? creationRequest = null)
    {
        if (creationRequest?.RequestedType is not { GenericTypeArguments.Length: 1 } listType)
        {
            return null;
        }

        var elementType = listType.GenericTypeArguments[0];
        
        var list = (IList) Activator.CreateInstance(creationRequest.Value.RequestedType)!;
        for (var i = 0; i < CollectionCount; i++)
        {
            list.Add(p.Resolve(elementType))   
        }
    }
    
    public void Apply(DataProviderBuilder builder)
    {
        builder.Register(typeof(List<>),);
    }
}