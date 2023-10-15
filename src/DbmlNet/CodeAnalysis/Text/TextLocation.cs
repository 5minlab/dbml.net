using System;

namespace DbmlNet.CodeAnalysis.Text;

/// <summary>
/// Represents a text location.
/// </summary>
public readonly struct TextLocation : IEquatable<TextLocation>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextLocation"/>.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="span">The text span.</param>
    public TextLocation(SourceText text, TextSpan span)
    {
        Text = text;
        Span = span;
    }

    /// <summary>
    /// Gets the source text.
    /// </summary>
    public SourceText Text { get; }

    /// <summary>
    /// Gets the text span.
    /// </summary>
    public TextSpan Span { get; }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    public readonly string FileName => Text.FileName;

    /// <summary>
    /// Gets the start of the line.
    /// </summary>
    public readonly int StartLine => Text.GetLineIndex(Span.Start);

    /// <summary>
    /// Gets the start of the character within the line.
    /// </summary>
    public readonly int StartCharacter => Span.Start - Text.Lines[StartLine].Start;

    /// <summary>
    /// Gets the end of the line.
    /// </summary>
    public readonly int EndLine => Text.GetLineIndex(Span.End);

    /// <summary>
    /// Gets the end of the character within the line.
    /// </summary>
    public readonly int EndCharacter => Span.End - Text.Lines[EndLine].Start;

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Text.GetHashCode() ^ Span.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is TextLocation span && Equals(span);
    }

    /// <summary>
    /// Determines whether the current instance of the <see cref="TextLocation"/> class is equal to given <see cref="TextLocation"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="TextLocation"/> instance to compare with the current instance.</param>
    /// <returns>True if the current instance is equal to the other instance; otherwise, false.</returns>
    public bool Equals(TextLocation other)
    {
        return Text == other.Text && Span == other.Span;
    }

    /// <summary>
    /// Determines whether two <see cref="TextLocation"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="TextLocation"/> instance to compare.</param>
    /// <param name="right">The second <see cref="TextLocation"/> instance to compare.</param>
    /// <returns>True if the two instances are equal; otherwise, false.</returns>
    public static bool operator ==(TextLocation left, TextLocation right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two <see cref="TextLocation"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="TextLocation"/> instance to compare.</param>
    /// <param name="right">The second <see cref="TextLocation"/> instance to compare.</param>
    /// <returns>True if the two instances are not equal; otherwise, false.</returns>
    public static bool operator !=(TextLocation left, TextLocation right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Returns the span text of this <see cref="TextLocation"/>.
    /// </summary>
    /// <returns>The span text of this <see cref="TextLocation"/>.</returns>
    public override string ToString() => Span.ToString();
}
