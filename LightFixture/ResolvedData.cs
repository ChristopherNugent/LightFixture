namespace LightFixture;

/// <summary>
/// Represents a unit of resolved data.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct ResolvedData<T>
{
    /// <summary>
    /// Whether data was successfully resolved.
    /// </summary>
    public bool IsResolved { get; private init; }
    
    /// <summary>
    /// The resolved value, or default.
    /// </summary>
    public T Value { get; private init; }

    public static implicit operator ResolvedData<T>(T value) => new()
    {
        IsResolved = true,
        Value = value
    };

    public static readonly ResolvedData<T> NoData = new() { IsResolved = false };
    
    public static implicit operator T(ResolvedData<T> resolvedData) => resolvedData.Value;
}

public static class ResolvedData
{
    public static ResolvedData<T> FromValue<T>(T value) => value;
    
    internal static ResolvedData<object> AsNonGeneric<T>(this ResolvedData<T> resolvedData) => resolvedData.IsResolved
        ? resolvedData.Value!
        : ResolvedData<object>.NoData;

    internal static ResolvedData<T> AsGeneric<T>(this ResolvedData<object> resolvedData) => resolvedData.IsResolved
        ? (T)resolvedData.Value
        : ResolvedData<T>.NoData;
}