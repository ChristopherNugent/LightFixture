namespace LightFixture;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DataFactoryIgnorePropertyAttribute(Type type, string propertyName) : Attribute;