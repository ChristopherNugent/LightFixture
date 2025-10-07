using System.Threading;
using LightFixture.SourceGeneration.Constants;
using Microsoft.CodeAnalysis;

namespace LightFixture.SourceGeneration;

internal sealed class DataFactoryDefinitionFactory
{
    public DataFactoryDefinition GetFactoryDefinition(INamedTypeSymbol symbol, CancellationToken token)
    {
        var definition = new DataFactoryDefinition();
        foreach (var attribute in symbol.GetAttributes())
        {
            token.ThrowIfCancellationRequested();
            switch (attribute.AttributeClass?.ToDisplayString())
            {
                case WellKnownTypes.DataFactoryAttribute:
                    HandleDataFactoryAttribute(definition, attribute);
                    break;
                case WellKnownTypes.DataFactoryIgnorePropertyAttribute:
                    HandleIgnorePropertyAttribute(definition, attribute);
                    break;
            }
        }

        return definition;
    }

    private static void HandleDataFactoryAttribute(DataFactoryDefinition definition, AttributeData data)
    {
        if (data.ConstructorArguments.Length is 1
            && data.ConstructorArguments[0].Value is ITypeSymbol type)
        {
            definition.RootTypes.Add(type);
        }
    }

    private static void HandleIgnorePropertyAttribute(DataFactoryDefinition definition, AttributeData data)
    {
        if (data.ConstructorArguments.Length is 2
            && data.ConstructorArguments[0].Value is ITypeSymbol type
            && data.ConstructorArguments[1].Value is string property)
        {
            if (!definition.IgnoredProperties.TryGetValue(type, out var ignored))
            {
                ignored = new();
                definition.IgnoredProperties[type] = ignored;
            }

            ignored.Add(property);
        }
    }
}