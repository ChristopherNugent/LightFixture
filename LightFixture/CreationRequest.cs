namespace LightFixture;

public sealed record CreationRequest(string? PropertyName)
{
    public static readonly CreationRequest Empty = new((string?) null);
}