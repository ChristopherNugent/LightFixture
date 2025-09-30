namespace LightFixture;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DataFactoryAttribute(Type type) : Attribute;