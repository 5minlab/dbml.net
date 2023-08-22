namespace DbmlNet.CodeAnalysis.Text;

/// <summary>
/// </summary>
public sealed class TextLine
{
    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <param name="lengthIncludingLineBreak"></param>
    public TextLine(SourceText text, int start, int length, int lengthIncludingLineBreak)
    {
        Text = text;
        Start = start;
        Length = length;
        LengthIncludingLineBreak = lengthIncludingLineBreak;
    }

    /// <summary>
    /// </summary>
    public SourceText Text { get; }

    /// <summary>
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// </summary>
    public int End => Start + Length;

    /// <summary>
    /// </summary>
    public int LengthIncludingLineBreak { get; }

    /// <summary>
    /// </summary>
    public TextSpan Span => new TextSpan(Start, Length);

    /// <summary>
    /// </summary>
    public TextSpan SpanIncludingLineBreak => new TextSpan(Start, LengthIncludingLineBreak);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Text.ToString(Span);
}
