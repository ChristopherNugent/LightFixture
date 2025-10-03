namespace LightFixture.Providers;

internal sealed class BooleanProvider : IDataProviderCustomization
{
    private readonly Random _random = new Random(2025_10_02); // Fixed seed for reproducible runs
    
    public void Apply(DataProviderBuilder builder)
    {
        builder.Register<bool>((_, _) => _random.Next() % 2 is 0);
    }
}