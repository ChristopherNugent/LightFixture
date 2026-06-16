namespace LightFixture;

/// <summary>
/// Defines different behaviors for dealing with obsolete members.
/// </summary>
public enum ObsoleteHandlingBehavior
{
    /// <summary>
    /// No special handling.
    /// </summary>
    None,
    
    /// <summary>
    /// Ignore obsolete properties when generating factories.
    /// </summary>
    IgnoreObsolete,
    
    /// <summary>
    /// Suppress warnings from obsolete members in generated code.
    /// </summary>
    SuppressWarnings,
    
    /// <summary>
    /// Suppress errors from obsolete members in generated code.
    /// </summary>
    SuppressErrors,
}