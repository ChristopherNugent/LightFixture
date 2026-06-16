#pragma warning disable CS9113 // Parameter is unread - This is just a target for a source generator.
namespace LightFixture;

/// <summary>
/// Mark a type for source generation as a data factory. Specify multiple on a single type if needed.
/// </summary>
/// <param name="type">The root type to use for source generation. All properties under this will be included.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DataFactoryAttribute(Type type) : Attribute;