namespace LightFixture;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class DataFactoryObsoleteHandlingAttribute(ObsoleteHandlingBehavior behavior) : Attribute;