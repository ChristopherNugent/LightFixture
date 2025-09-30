namespace LightFixture.Providers;

internal sealed class StringProvider : IDataProviderCustomization
{
    public static readonly StringProvider Instance = new();

    private StringProvider()
    {
    }

    public void Apply(DataProviderBuilder builder)
    {
        builder.Register<string>(static (_, request) => request.PropertyName switch
        {
            null => Guid.NewGuid().ToString(),
            _ => $"{request.PropertyName}_{Guid.NewGuid()}"
        });
    }
}