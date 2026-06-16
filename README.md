# About

LightFixture is intended a faster (if less flexible) replacement for libraries like AutoFixture. While those libraries
are excellent, they can become slow as the amount of anonymous data increases.

LightFixture provides source-generated factories and enables user-provided implementations to be provided for resolving
anonymous data.

# Installation

LightFixture is available as `CNugent.LightFixture`.

You can add it to your project with `dotnet add {project} package CNugent.LightFixture`.

# Benchmark compared to AutoFixture

This is based on the `LightFixture.Benchmark` project within this repo.

| Method                      | Mean              | Error            | StdDev           | Ratio    | RatioSD | Gen0       | Gen1      | Allocated   | Alloc Ratio |
|---------------------------- |------------------:|-----------------:|-----------------:|---------:|--------:|-----------:|----------:|------------:|------------:|
| NarrowOfNarrow_LightFixture |         759.47 ns |        11.508 ns |        10.764 ns |     1.00 |    0.02 |     0.0353 |         - |       592 B |        1.00 |
| NarrowOfNarrow_AutoFixture  |     229,561.81 ns |     4,406.875 ns |     4,715.307 ns |   302.32 |    7.33 |    12.2070 |         - |    205098 B |      346.45 |
|                             |                   |                  |                  |          |         |            |           |             |             |
| NarrowOfWide_LightFixture   |      17,062.51 ns |       127.730 ns |       113.230 ns |     1.00 |    0.01 |     0.5188 |         - |      8728 B |        1.00 |
| NarrowOfWide_AutoFixture    |   6,652,767.72 ns |   122,484.995 ns |   108,579.758 ns |   389.92 |    6.63 |   320.3125 |    7.8125 |   5476059 B |      627.41 |
|                             |                   |                  |                  |          |         |            |           |             |             |
| NarrowStruct_LightFixture   |          71.82 ns |         1.361 ns |         1.337 ns |     1.00 |    0.03 |     0.0157 |         - |       264 B |        1.00 |
| NarrowStruct_AutoFixture    |      72,035.54 ns |     1,405.807 ns |     1,314.993 ns | 1,003.34 |   25.17 |     3.7842 |         - |     65000 B |      246.21 |
|                             |                   |                  |                  |          |         |            |           |             |             |
| WideOfNarrow_LightFixture   |      22,003.40 ns |       209.782 ns |       196.231 ns |     1.00 |    0.01 |     0.6714 |         - |     11456 B |        1.00 |
| WideOfNarrow_AutoFixture    |   7,622,914.87 ns |   144,245.828 ns |   148,129.846 ns |   346.47 |    7.20 |   398.4375 |    7.8125 |   6682614 B |      583.33 |
|                             |                   |                  |                  |          |         |            |           |             |             |
| WideOfWide_LightFixture     |     574,385.48 ns |     7,731.244 ns |     7,231.810 ns |     1.00 |    0.02 |    16.6016 |    0.9766 |    282656 B |        1.00 |
| WideOfWide_AutoFixture      | 230,612,500.53 ns | 4,298,159.443 ns | 4,020,501.012 ns |   401.55 |    8.33 | 10000.0000 | 1000.0000 | 182904912 B |      647.09 |

# Usage

To get started with source generated providers, you can use something like the following

```csharp
[DataFactory(typeof(SampleData)]
public partial class SampleDataProvider;

public sealed class SampleData
{
    public int Int { get; set; }
    
    public double? Double { get; set; }
}
```

Marking generates a data provider customization automatically generates all needed factories for the type trees
specified. Multiple attributes can be specified together .

Data can be resolved from a `DataProvider` like so

```csharp
var dataProvider = new DataProviderBuilder()
    .Customize(new SampleDataProvider())
    .Build();

var data = dataProvider.Resolve<SampleData>().Value;
```

To provide your own customizations, you can write something like

```csharp
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
```

or register more directly by calling either of the following on `DataProviderBuilder`

```csharp
public DataProviderBuilder Register<T>(
    Func<DataProvider, CreationRequest?, ResolvedData<T>> factory,
    bool overrideExisting = false)
    
public DataProviderBuilder Register(
    Type type,
    Func<DataProvider, CreationRequest?, ResolvedData<object>> factory,
    bool overrideExisting = false)
```

Postprocessors for more granular customization can be added like so.

```csharp
// specified type
builder.AddPostProcessor<BasicType>((dataProvider, bt) => bt.Value = 42);

// or less specically
buider.AddPostProcessor((dataProvider, obj) => 
{
    if(obj is BasicType bt) 
    {
        bt.Value = 42;
    }
});
```

Properties can be ignored during source generation like so.

```csharp
[DataFactory(typeof(SampleData)]
[DataFactoryIgnoreProperty(typeof(SampleData), nameof(SampleData.Double))]
public partial class SampleDataProvider;

public sealed class SampleData
{
    public int Int { get; set; }
    
    public double? Double { get; set; }
}
```

LightFixture supports registration of open generics

```csharp
public sealed record Wrapper<T>(T Value);

// Enables resolution of Wrapper<string>, Wrapper<int>, etc.
[DataFactory(typeof(Wrapper<>))]
public partial class SampleDataProvider;
```