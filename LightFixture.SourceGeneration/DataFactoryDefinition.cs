using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace LightFixture.SourceGeneration;

internal sealed class DataFactoryDefinition
{
    public HashSet<ITypeSymbol> RootTypes { get; } = new(SymbolEqualityComparer.Default);

    public Dictionary<ITypeSymbol, HashSet<string>> IgnoredProperties { get; } = new(SymbolEqualityComparer.Default);
}