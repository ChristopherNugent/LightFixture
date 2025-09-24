namespace LightFixture;

public record struct CreationRequest(Type? RequestedType, string? PropertyName)
{
    public static readonly CreationRequest Empty = new(null, null);
}