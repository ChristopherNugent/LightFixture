namespace LightFixture;

public record struct CreationRequest(string? PropertyName)
{
    public static readonly CreationRequest Empty = new((string?) null);
}