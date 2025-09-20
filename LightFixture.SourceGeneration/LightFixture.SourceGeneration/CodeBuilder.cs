using System.Text;

namespace LightFixture.SourceGeneration;

internal sealed class CodeBuilder
{
    private readonly StringBuilder _builder = new();

    private int _indentationLevel = 0;
    private bool _hasLineBeenIndented = false;

    public void AppendLine()
    {
        _builder.AppendLine();
        _hasLineBeenIndented = false;
    }

    public void AppendLine(string text)
    {
     
        _builder.AppendLine(text);
        _hasLineBeenIndented = false;
    }

    private void EnsureIndented()
    {
        if (!_hasLineBeenIndented) return;
        for (var i = 0; i < _indentationLevel; i++)
        {
            _builder.Append('\t');
        }
    }
}