using System.Collections.Concurrent;

namespace LightFixture.Providers;

internal sealed class EnumProvider : IDataProviderCustomization
{
    public static readonly EnumProvider Instance = new EnumProvider();

    private readonly ConcurrentDictionary<Type, IEnumProvider> _factories = new();

    private EnumProvider()
    {
    }

    private ResolvedData<object> GetEnum(DataProvider dataProvider, CreationRequest creationRequest)
    {
        if (creationRequest.RequestedType?.IsEnum is not true)
        {
            return ResolvedData<object>.NoData;
        }
        var enumType = creationRequest.RequestedType;

        if (!_factories.TryGetValue(creationRequest.RequestedType, out var enumProvider))
        {
            enumProvider = (IEnumProvider) Activator.CreateInstance(typeof(TypedEnumProvider<>).MakeGenericType(enumType))!;
            _factories.TryAdd(creationRequest.RequestedType, enumProvider);
        }

        return enumProvider.Get();
    }
    
    public void Apply(DataProviderBuilder builder)
    {
        builder.Register(GetEnum);
    }
    
    private interface IEnumProvider
    {
        object Get();
    }

    private sealed class TypedEnumProvider<T> : IEnumProvider where T : struct, Enum
    {
        private readonly T[] _values = Enum.GetValues<T>();
        private long _iteration;
        
        public object Get()
        {
            return _values[_iteration++ % _values.Length];
        }
    }
}