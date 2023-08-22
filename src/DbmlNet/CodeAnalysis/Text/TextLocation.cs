using System;

namespace DbmlNet.CodeAnalysis.Text;

/// <summary>
/// </summary>
public readonly struct TextLocation : IEquatable<TextLocation>
{
    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="span"></param>
    public TextLocation(SourceText text, TextSpan span)
    {
        Text = text;
        Span = span;
    }

    /// <summary>
    /// </summary>
    public SourceText Text { get; }

    /// <summary>
    /// </summary>
    public TextSpan Span { get; }

    /// <summary>
    /// </summary>
    public readonly string FileName => Text.FileName;

    /// <summary>
    /// </summary>
    public readonly int StartLine => Text.GetLineIndex(Span.Start);

    /// <summary>
    /// </summary>
    public readonly int StartCharacter => Span.Start - Text.Lines[StartLine].Start;

    /// <summary>
    /// </summary>
    public readonly int EndLine => Text.GetLineIndex(Span.End);

    /// <summary>
    /// </summary>
    public readonly int EndCharacter => Span.End - Text.Lines[EndLine].Start;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return Text.GetHashCode() ^ Span.GetHashCode();
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return obj is TextLocation span && Equals(span);
    }

    /// <summary>
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(TextLocation other)
    {
        return Text == other.Text && Span == other.Span;
    }

    /// <summary>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(TextLocation left, TextLocation right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(TextLocation left, TextLocation right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Span.ToString();
}
