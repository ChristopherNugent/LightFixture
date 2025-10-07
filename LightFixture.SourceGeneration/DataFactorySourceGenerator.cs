using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LightFixture.SourceGeneration.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LightFixture.SourceGeneration;

[Generator]
public sealed class DataFactorySourceGenerator : IIncrementalGenerator
{
    private static readonly DataFactoryDefinitionFactory DefinitionFactory = new();
    private static readonly DataFactoryWriter FactoryWriter = new();
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var targetSymbols = context.SyntaxProvider.ForAttributeWithMetadataName(
            WellKnownTypes.DataFactoryAttribute,
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

        var factoryDefinition = DefinitionFactory.GetFactoryDefinition(namedType, context.CancellationToken);
        var sourceText = FactoryWriter.WriteFactory(factoryDefinition, context.CancellationToken);
        context.AddSource(namedType.Name + ".g.cs", sourceText);
    }
}