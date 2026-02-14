using System.Collections.Generic;
using System.Linq;
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
            var members = type.GetMembers();
            foreach (var member in members)
            {
                if (member is not IPropertySymbol { GetMethod: not null, SetMethod: not null } property
                    || IsIgnored(type, property.Name))
                {
                    continue;
                }

                var relevantTypes = UnwrapType(property.Type);

                foreach (var relevantType in relevantTypes)
                {
                    EnqueueType(relevantType);
                }
            }
        }

        yield break;

        void EnqueueType(ITypeSymbol t)
        {
            if (!IsNativeType(t)
                && !IsIgnored(t)
                && explored.Add(t))
            {
                queue.Enqueue(t);
            }
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
    
    private static IEnumerable<ITypeSymbol> UnwrapType(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol { IsGenericType: true } generic)
        {
            return generic.TypeArguments.SelectMany(UnwrapType);
        }

        if (type is IArrayTypeSymbol array)
        {
            return [array.ElementType];
        }

        return [type];
    }
}