using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Text;

/// <summary>
/// Represents a source text.
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
    /// Gets the lines of the source text.
    /// </summary>
    public ImmutableArray<TextLine> Lines { get; }

    /// <summary>
    /// Gets the length of the source text.
    /// </summary>
    public int Length => _text.Length;

    /// <summary>
    /// Gets the file name of the source text.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Gets or sets the character at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the character to get or set.</param>
    /// <returns>The character at the specified index.</returns>
    public char this[int index] => _text[index];

    /// <summary>
    /// Creates a new instance of SourceText from the specified text and optional file name.
    /// </summary>
    /// <param name="text">The text content of the source.</param>
    /// <param name="fileName">The optional file name.</param>
    /// <returns>A new instance of SourceText.</returns>
    public static SourceText From(string text, string fileName = "")
    {
        return new SourceText(text, fileName);
    }

    /// <summary>
    /// Gets the line index of the specified position.
    /// </summary>
    /// <param name="position">The position value.</param>
    /// <returns>The line index.</returns>
    public int GetLineIndex(int position)
    {
        int lower = 0;
        int upper = Lines.Length - 1;

        while (lower <= upper)
        {
            int index = lower + ((upper - lower) / 2);
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
    /// Returns a string representation of the current instance.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => _text;

    /// <summary>
    /// Returns a string representation of the specified portion of the current instance.
    /// </summary>
    /// <param name="start">The zero-based starting character position of the substring.</param>
    /// <param name="length">The number of characters in the substring.</param>
    /// <returns>A string that represents the specified portion of the current object.</returns>
    public string ToString(int start, int length) => _text.Substring(start, length);

    /// <summary>
    /// Returns a string representation of the specified portion of the current instance.
    /// </summary>
    /// <param name="span">The TextSpan specifying the portion of the string to retrieve.</param>
    /// <returns>A string that represents the specified portion of the current object.</returns>
    public string ToString(TextSpan span) => ToString(span.Start, span.Length);

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
        TextLine line = new(sourceText, lineStart, lineLength, lineLengthIncludingLineBreak);
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
}
