namespace LightFixture.Providers;

internal sealed class DateTimeProvider : IDataProviderCustomization
{
    private const long TicksInYear = 315360000000000;
    private readonly Random _random = new Random(2025_10_02);

    public void Apply(DataProviderBuilder builder)
    {
        builder.Register<DateTime>((_, _) =>
        {
            var range = _random.NextDouble() * 2 - 1;
            var ticksToModify = TicksInYear * range;
            return DateTime.UtcNow.AddTicks((long)ticksToModify);
        });
        builder.Register<DateTimeOffset>((_, _) =>
        {
            var range = _random.NextDouble() * 2 - 1;
            var ticksToModify = TicksInYear * range;
            return DateTimeOffset.UtcNow.AddTicks((long)ticksToModify);
        });
        builder.Register<DateOnly>((p, _) => DateOnly.FromDateTime(p.Resolve<DateTime>()));
        builder.Register<TimeOnly>((p, _) => TimeOnly.FromDateTime(p.Resolve<DateTime>()));
    }
}