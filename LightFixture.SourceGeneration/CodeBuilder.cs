using System;
using System.Text;

namespace LightFixture.SourceGeneration;

internal sealed class CodeBuilder
{
    private readonly StringBuilder _builder = new();

    private int _indentationLevel = 0;
    private bool _hasLineBeenIndented = false;

    public CodeBuilder AppendLine()
    {
        _builder.AppendLine();
        _hasLineBeenIndented = false;
        return this;
    }

    public CodeBuilder AppendLine(string text)
    {
        EnsureIndented();
        _builder.AppendLine(text);
        _hasLineBeenIndented = false;
        return this;
    }

    public CodeBuilder Append(string text)
    {
        EnsureIndented();
        _builder.Append(text);
        return this;
    }

    public CodeBuilder Indent()
    {
        _indentationLevel++;
        return this;
    }

    public CodeBuilder Outdent()
    {
        _indentationLevel = Math.Max(0, _indentationLevel - 1);
        return this;
    }

    public CodeBuilder OpenBlock(string opener = "{") => AppendLine(opener).Indent();

    public CodeBuilder CloseBlock(string closer = "}") => Outdent().AppendLine(closer);

    private void EnsureIndented()
    {
        if (_hasLineBeenIndented)
        {
            return;
        }
        for (var i = 0; i < _indentationLevel; i++)
        {
            _builder.Append('\t');
        }
        _hasLineBeenIndented = true;
    }

    public override string ToString() => _builder.ToString();
}