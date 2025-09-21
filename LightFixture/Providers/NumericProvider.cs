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
            .Register<int>(() => (int) Interlocked.Increment(ref _iteration))
            .Register<long>(() => Interlocked.Increment(ref _iteration))
            .Register<double>(() => (double) Interlocked.Increment(ref _iteration))
            .Register<float>(() => (float) Interlocked.Increment(ref _iteration))
            .Register<decimal>(() => Interlocked.Increment(ref _iteration));
    }
}