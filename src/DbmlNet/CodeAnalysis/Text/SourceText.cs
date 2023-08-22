using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Text;

/// <summary>
/// </summary>
public sealed class SourceText
{
    private readonly string _text;

    private SourceText(string text, string fileName)
    {
        _text = text;
        FileName = fileName;
        Lines = ParseLines(this, text);
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static SourceText From(string text, string fileName = "")
    {
        return new SourceText(text, fileName);
    }

    /// <summary>
    /// </summary>
    /// <param name="sourceText"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text)
    {
        ImmutableArray<TextLine>.Builder result =
            ImmutableArray.CreateBuilder<TextLine>();

        int position = 0;
        int lineStart = 0;

        while (position < text.Length)
        {
            int lineBreakWidth = GetLineBreakWidth(text, position);

            if (lineBreakWidth == 0)
            {
                position++;
            }
            else
            {
                AddLine(result, sourceText, position, lineStart, lineBreakWidth);

                position += lineBreakWidth;
                lineStart = position;
            }
        }

        if (position >= lineStart)
            AddLine(result, sourceText, position, lineStart, 0);

        return result.ToImmutable();
    }

    private static void AddLine(ImmutableArray<TextLine>.Builder result, SourceText sourceText, int position, int lineStart, int lineBreakWidth)
    {
        int lineLength = position - lineStart;
        int lineLengthIncludingLineBreak = lineLength + lineBreakWidth;
        TextLine line = new TextLine(sourceText, lineStart, lineLength, lineLengthIncludingLineBreak);
        result.Add(line);
    }

    private static int GetLineBreakWidth(string text, int position)
    {
        char c = text[position];
        char l = position + 1 >= text.Length ? '\0' : text[position + 1];

        if (c == '\r' && l == '\n')
            return 2;

        if (c == '\r' || c == '\n')
            return 1;

        return 0;
    }

    /// <summary>
    /// </summary>
    public ImmutableArray<TextLine> Lines { get; }

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>

    public char this[int index] => _text[index];

    /// <summary>
    /// </summary>
    public int Length => _text.Length;

    /// <summary>
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public int GetLineIndex(int position)
    {
        int lower = 0;
        int upper = Lines.Length - 1;

        while (lower <= upper)
        {
            int index = lower + (upper - lower) / 2;
            int start = Lines[index].Start;

            if (position == start)
                return index;

            if (start > position)
            {
                upper = index - 1;
            }
            else
            {
                lower = index + 1;
            }
        }

        return lower - 1;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => _text;

    /// <summary>
    /// </summary>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public string ToString(int start, int length) => _text.Substring(start, length);

    /// <summary>
    /// </summary>
    /// <param name="span"></param>
    /// <returns></returns>
    public string ToString(TextSpan span) => ToString(span.Start, span.Length);
}
