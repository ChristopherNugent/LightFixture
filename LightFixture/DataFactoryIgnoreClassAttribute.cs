#pragma warning disable CS9113 // Parameter is unread - This is just a target for a source generator.

namespace LightFixture;

/// <summary>
/// Specify that a type should be excluded during factory code generation.
/// </summary>
/// <param name="type">The type to ignore.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DataFactoryIgnoreClassAttribute(Type type) : Attribute;