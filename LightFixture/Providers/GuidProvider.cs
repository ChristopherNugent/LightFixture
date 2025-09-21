namespace LightFixture.Providers;

internal sealed class GuidProvider : IDataProviderCustomization
{
    public static readonly GuidProvider Instance = new();

    private GuidProvider()
    {
    }

    public void Apply(DataProviderBuilder builder)
    {
        builder.Register<Guid>(static (_, _) => Guid.NewGuid());
    }
}