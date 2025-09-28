# About

LightFixture is intended a faster (if less flexible) replacement for libraries like AutoFixture. While those libraries
are excellent, they can become slow as the amount of anonymous data increases.

LightFixture provides source-generated factories and enables user-provided implementations to be provided for resolving
anonymous data.

# Usage

To get started with source generated providers, you can use something like the following

```csharp
[DataFactory]
public partial class SampleDataProvider
{
    [DataFactory]
    private partial SampleData SomeData();
}

public sealed class SampleData
{
    public int Int { get; set; }
    
    public double? Double { get; set; }
}
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

Postprocessors for more granular customization can be added like so

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

Data can be resolved from a `DataProvider` like so

```csharp
var dataProvider = new DataProviderBuilder()
    .Customize(new SampleDataProvider())
    .Build();

var data = dataProvider.Resolve<SampleData>().Value;
```