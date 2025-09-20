namespace LightFixture.Providers;

internal sealed class GuidProvider : IDataProviderCustomization
{
    public static readonly GuidProvider Instance = new GuidProvider();

    private GuidProvider()
    {
    }

    public void Apply(DataProviderBuilder builder)
    {
        builder.Register<Guid>((_, _) => Guid.NewGuid());
    }
}