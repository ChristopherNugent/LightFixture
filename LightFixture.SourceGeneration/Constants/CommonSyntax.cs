namespace LightFixture.SourceGeneration.Constants;

internal static class CommonSyntax
{
    private const string PackageName = "CNugent.LightFixture";

    private const string Version = "0.4.0";
    
    public const string GeneratedCodeAttribute = $"[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"{PackageName}\", \"{Version}\")]";
}