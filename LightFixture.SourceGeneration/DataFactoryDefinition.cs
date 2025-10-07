using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace LightFixture.SourceGeneration;

internal sealed class DataFactoryDefinition(INamedTypeSymbol factorySymbol)
{
    public INamedTypeSymbol FactoryType { get; } = factorySymbol;

    public HashSet<ITypeSymbol> RootTypes { get; } = new(SymbolEqualityComparer.Default);

    public Dictionary<ITypeSymbol, HashSet<string>> IgnoredProperties { get; } = new(SymbolEqualityComparer.Default);

    public HashSet<ITypeSymbol> IgnoredTypes { get; } = new(SymbolEqualityComparer.Default);

    public IEnumerable<ITypeSymbol> WalkTypes()
    {
        var explored = new HashSet<ITypeSymbol>(RootTypes, SymbolEqualityComparer.Default);
        var queue = new Queue<ITypeSymbol>(RootTypes);

        while (queue.Count > 0)
        {
            var type = queue.Dequeue();
            yield return type;
            foreach (var member in type.GetMembers())
            {
                if (member is not IPropertySymbol { GetMethod: not null, SetMethod: not null } property
                    || IsIgnored(type, property.Name))
                {
                    continue;
                }

                var relevantType = UnwrapType(property.Type);

                if (!IsNativeType(relevantType)
                    && !IsIgnored(type)
                    && explored.Add(relevantType))
                {
                    queue.Enqueue(relevantType);
                }
            }
        }

        yield break;

        static ITypeSymbol UnwrapType(ITypeSymbol type)
        {
            if (type is INamedTypeSymbol { IsGenericType: true, Name: "Nullable" } nullable)
            {
                return UnwrapType(nullable.TypeArguments[0]);
            }

            if (type is IArrayTypeSymbol array)
            {
                return array.ElementType;
            }

            return type;
        }
    }

    private bool IsIgnored(ITypeSymbol containingType, string propertyName)
    {
        return IgnoredProperties.TryGetValue(containingType, out var ignored)
               && ignored.Contains(propertyName);
    }

    private bool IsIgnored(ITypeSymbol type) => IgnoredTypes.Contains(type);

    private static bool IsNativeType(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol { IsGenericType: true, Name: "Nullable" } nullable)
        {
            type = nullable.TypeArguments[0];
        }

        if (type is IArrayTypeSymbol)
        {
            return true;
        }

        if (type.SpecialType is not SpecialType.None)
        {
            return true;
        }

        if (type.TypeKind is TypeKind.Enum)
        {
            return true;
        }

        if (type.ContainingNamespace?.ToDisplayString() is "System.Collections.Generic")
        {
            return true;
        }

        return false;
    }
}