namespace LightFixture.Providers;

internal sealed class NumericProvider : IDataProviderCustomization
{
    public static readonly NumericProvider Instance = new();
    
    private long _iteration;

    private NumericProvider()
    {
    }
        
    public void Apply(DataProviderBuilder builder)
    {
        builder
            .Register<int>((_, _) => (int) Interlocked.Increment(ref _iteration))
            .Register<long>((_, _) => Interlocked.Increment(ref _iteration))
            .Register<double>((_, _) => (double) Interlocked.Increment(ref _iteration))
            .Register<float>((_, _) => (float) Interlocked.Increment(ref _iteration))
            .Register<decimal>((_, _) => Interlocked.Increment(ref _iteration));
    }
}