using Microsoft.CodeAnalysis;

namespace LightFixture.SourceGeneration.Extensions;

internal static class AccessibilityExtensions
{
    public static string ToSyntax(this Accessibility accessibility)
        => accessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.Protected => "protected",
            Accessibility.Internal => "internal",
            Accessibility.Private  => "private",
           _ => string.Empty,
        };
}