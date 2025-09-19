namespace LightFixture;

public readonly struct ResolvedData<T>
{
    public bool IsResolved { get; private init; }
    public T Value { get; private init; }

    public static implicit operator ResolvedData<T>(T value) => new()
    {
        IsResolved = true,
        Value = value
    };

    public static readonly ResolvedData<T> NoData = new() { IsResolved = false };
    
    public static implicit operator T(ResolvedData<T> resolvedData) => resolvedData.Value;
}