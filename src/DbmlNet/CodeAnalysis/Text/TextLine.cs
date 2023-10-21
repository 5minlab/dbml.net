namespace DbmlNet.CodeAnalysis.Text;

/// <summary>
/// Represents a text line.
/// </summary>
public sealed class TextLine
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextLine"/> class.
    /// </summary>
    /// <param name="text">The source text containing the line.</param>
    /// <param name="start">The starting position of the line within the source text.</param>
    /// <param name="length">The length of the line.</param>
    /// <param name="lengthIncludingLineBreak">The length of the line including the line break.</param>
    public TextLine(SourceText text, int start, int length, int lengthIncludingLineBreak)
    {
        Text = text;
        Start = start;
        Length = length;
        LengthIncludingLineBreak = lengthIncludingLineBreak;
    }

    /// <summary>
    /// Gets the source text.
    /// </summary>
    public SourceText Text { get; }

    /// <summary>
    /// Gets the start position of the line within the source text.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// Gets the length of the line.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets the end position of the line within the source text.
    /// </summary>
    public int End => Start + Length;

    /// <summary>
    /// Gets the length of the line including the line break.
    /// </summary>
    public int LengthIncludingLineBreak { get; }

    /// <summary>
    /// Gets the span of the line.
    /// </summary>
    public TextSpan Span => new TextSpan(Start, Length);

    /// <summary>
    /// Gets the span of the line including the line break.
    /// </summary>
    public TextSpan SpanIncludingLineBreak => new TextSpan(Start, LengthIncludingLineBreak);

    /// <summary>
    /// Returns the text of this line.
    /// </summary>
    /// <returns>The text of this line.</returns>
    public override string ToString() => Text.ToString(Span);
}
