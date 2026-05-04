#pragma warning disable CS9113 // Parameter is unread - This is just a target for a source generator.

namespace LightFixture;

/// <summary>
/// Specify that a property should be ignored during source generation.
/// </summary>
/// <param name="type">The type on which to ignore the property.</param>
/// <param name="propertyName">The property to ignore.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DataFactoryIgnorePropertyAttribute(Type type, string propertyName) : Attribute;