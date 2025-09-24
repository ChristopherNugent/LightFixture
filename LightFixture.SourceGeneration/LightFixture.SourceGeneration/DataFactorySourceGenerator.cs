using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LightFixture.SourceGeneration;

[Generator]
public sealed class DataFactorySourceGenerator : IIncrementalGenerator
{
    private const string AttributeFqn = "LightFixture.DataFactoryAttribute";
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targetSymbols = context.SyntaxProvider.ForAttributeWithMetadataName(
            AttributeFqn,
            (node, _) => node is ClassDeclarationSyntax,
            (ctx, _) => ctx.TargetSymbol);
        
        context.RegisterSourceOutput(targetSymbols, HandleSymbol);
    }

    private static void HandleSymbol(SourceProductionContext context, ISymbol symbol)
    {
        if (symbol is not INamedTypeSymbol namedType)
        {
            return;
        }
        
        var exploredTypes = new Dictionary<ISymbol, string>(SymbolEqualityComparer.Default);
    }
}