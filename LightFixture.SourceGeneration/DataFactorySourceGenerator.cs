using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

        var rootFactories = GetRootFactories(namedType, context.CancellationToken);
        var typesToGenerate = WalkTypes(rootFactories.Values);
        var factoryLookup = new Dictionary<ITypeSymbol, int>(SymbolEqualityComparer.Default);

        var code = new CodeBuilder();

        code
            .AppendLine("using LightFixture;")
            .AppendLine()
            .AppendLine($"namespace {namedType.ContainingNamespace.ToDisplayString()};")
            .AppendLine()
            .AppendLine($"partial class {namedType.Name} : global::LightFixture.IDataProviderCustomization")
            .OpenBlock();

        var factoryNumber = 0;
        foreach (var type in typesToGenerate)
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            WriteAnonymousFactory(type);
            factoryNumber++;
        }

        foreach (var kvp in rootFactories)
        {
            var methodName = kvp.Key;
            var returnType = kvp.Value;
            code.AppendLine($"public partial {GetFullTypeName(returnType)} {methodName}() => default;");
        }

        code.AppendLine("public void Apply(global::LightFixture.DataProviderBuilder builder)")
            .OpenBlock();
        foreach (var kvp in factoryLookup)
        {
            code.AppendLine($"builder.Register<{GetFullTypeName(kvp.Key)}>(static (p, _) => _Factory{kvp.Value}(p));");
        }

        code.CloseBlock();
        code.CloseBlock();

        context.AddSource(namedType.Name + ".cs", code.ToString());

        void WriteAnonymousFactory(ITypeSymbol type)
        {
            factoryLookup[type] = factoryNumber;

            var constructor = type is INamedTypeSymbol nt
                ? nt.Constructors
                    .Where(c => c is { IsStatic: false, DeclaredAccessibility: Accessibility.Public })
                    .OrderBy(x => x.Parameters.Length)
                    .FirstOrDefault()
                : null;
            var constructorParameters = constructor?.Parameters ?? ImmutableArray<IParameterSymbol>.Empty;
            var constructorParameterNames = new HashSet<string>(
                constructorParameters.Select(x => x.Name),
                StringComparer.InvariantCultureIgnoreCase);

            code.AppendLine(
                    $"private static {GetFullTypeName(type)} _Factory{factoryNumber}(global::LightFixture.DataProvider provider)")
                .OpenBlock()
                .Append($"var o = new {GetFullTypeName(type)}(");

            for (var i = 0; i < constructorParameters.Length; i++)
            {
                var parameter = constructorParameters[i];
                code.Append($"provider.Resolve<{GetFullTypeName(parameter.Type)}>(")
                    .Append("new global::LightFixture.CreationRequest(")
                    .Append($"typeof({GetFullTypeName(parameter.Type)}),")
                    .Append($"\"{parameter.Name}\")).Value");

                if (i < constructorParameters.Length - 1)
                {
                    code.Append(", ");
                }
            }

            code.AppendLine(");");

            var count = 0;
            foreach (var member in type.GetMembers())
            {
                if (member is not IPropertySymbol { GetMethod: not null, SetMethod: not null } property
                    || constructorParameterNames.Contains(property.Name))
                {
                    continue;
                }

                code.AppendLine($"var o{count} = provider.Resolve<{GetFullTypeName(property.Type)}>(")
                    .Indent()
                    .AppendLine("new global::LightFixture.CreationRequest(")
                    .Indent()
                    .AppendLine($"typeof({GetFullTypeName(property.Type)}),")
                    .AppendLine($"\"{property.Name}\"));")
                    .Outdent()
                    .Outdent()
                    .AppendLine($"if(o{count}.IsResolved)")
                    .OpenBlock()
                    .AppendLine($"o.{property.Name} = o{count}.Value;")
                    .CloseBlock();
                
                count++;
            }

            code.AppendLine("return o;")
                .CloseBlock("}")
                .AppendLine();
        }
    }

    private static string GetFullTypeName(ITypeSymbol type) => type switch
    {
        INamedTypeSymbol { IsGenericType: true, Name: "Nullable" } nullable => GetFullTypeName(
            nullable.TypeArguments[0]),
        { SpecialType: SpecialType.None, IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated }
            => $"global::{type.ToDisplayString()}".Replace("?", string.Empty),
        { SpecialType: SpecialType.None } => $"global::{type.ToDisplayString()}",
        { SpecialType: SpecialType.System_String } => "string",
        _ => type.ToDisplayString(),
    };

    private static Dictionary<string, ITypeSymbol> GetRootFactories(INamedTypeSymbol symbol, CancellationToken token)
    {
        var dict = new Dictionary<string, ITypeSymbol>();
        foreach (var member in symbol.GetMembers())
        {
            token.ThrowIfCancellationRequested();
            if (member is not IMethodSymbol
                {
                    Parameters.Length: 0, 
                    Name: not ".ctor",
                    IsPartialDefinition: true,
                } method)
            {
                continue;
            }

            dict.Add(method.Name, method.ReturnType);
        }

        return dict;
    }

    private static IEnumerable<ITypeSymbol> WalkTypes(IEnumerable<ITypeSymbol> symbols)
    {
        var explored = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        var queue = new Queue<ITypeSymbol>(symbols);

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            yield return item;
            var members = item.GetMembers();
            foreach (var member in members)
            {
                if (member is not IPropertySymbol { GetMethod: not null, SetMethod: not null } property)
                {
                    continue;
                }

                if (!IsNativeType(property.Type) && explored.Add(property.Type))
                {
                    queue.Enqueue(property.Type);
                }
            }
        }
    }

    private static bool IsNativeType(ITypeSymbol type)
    {
        if (type is INamedTypeSymbol { IsGenericType: true, Name: "Nullable" } nullable)
        {
            type = nullable.TypeArguments[0];
        }

        if (type.SpecialType is not SpecialType.None)
        {
            return true;
        }

        if (type.TypeKind is TypeKind.Enum)
        {
            return true;
        }

        if (type.ContainingNamespace.ToDisplayString() is "System.Collections.Generic")
        {
            return true;
        }

        return false;
    }
}