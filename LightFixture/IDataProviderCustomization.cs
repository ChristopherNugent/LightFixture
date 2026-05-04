namespace LightFixture;

/// <summary>
/// Defines a reusable unit of customization for a <see cref="DataProviderBuilder"/>
/// </summary>
public interface IDataProviderCustomization
{
    void Apply(DataProviderBuilder builder);
}