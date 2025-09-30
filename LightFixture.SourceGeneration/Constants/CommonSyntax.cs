namespace LightFixture.SourceGeneration.Constants;

internal static class CommonSyntax
{
    private const string PackageName = "CNugent.LightFixture";

    private const string Version = "0.3.0";
    
    public const string GeneratedCodeAttribute = $"[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"{PackageName}\", \"{Version}\")]";
}