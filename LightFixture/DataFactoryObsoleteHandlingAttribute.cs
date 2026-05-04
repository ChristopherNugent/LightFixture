namespace LightFixture;

/// <summary>
/// Enables application of common patterns for dealing with Obsolete members.
/// </summary>
/// <param name="behavior">The behavior to apply.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class DataFactoryObsoleteHandlingAttribute(ObsoleteHandlingBehavior behavior) : Attribute;