namespace LightFixture.Providers;

internal sealed class NumericProvider : IDataProviderCustomization
{
    public static readonly NumericProvider Instance = new();
    
    private long _iteration;

    private NumericProvider()
    {
    }

    private byte GetByte() => (byte)(Interlocked.Increment(ref _iteration) % (byte.MaxValue + 1L));
    
    private short GetShort() => (short)(Interlocked.Increment(ref _iteration) % (short.MaxValue + 1));
    
    private int GetInt() => (int)(Interlocked.Increment(ref _iteration) % (int.MaxValue + 1L));

    private long GetLong() => Interlocked.Increment(ref _iteration);
    
    private float  GetFloat() => Interlocked.Increment(ref _iteration);
    
    private double GetDouble() => Interlocked.Increment(ref _iteration);
    
    private decimal GetDecimal() => Interlocked.Increment(ref _iteration);
        
    public void Apply(DataProviderBuilder builder)
    {
        builder
            .Register<byte>((_, _) => GetByte())
            .Register<short>((_, _) => GetShort())
            .Register<int>((_, _) => GetInt())
            .Register<long>((_, _) => GetLong())
            .Register<float>((_, _) => GetFloat())
            .Register<double>((_, _) => GetDouble())
            .Register<decimal>((_, _) => GetDecimal());
    }
}