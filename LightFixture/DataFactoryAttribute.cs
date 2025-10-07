#pragma warning disable CS9113 // Parameter is unread - This is just a target for a source generator.
namespace LightFixture;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DataFactoryAttribute(Type type) : Attribute;