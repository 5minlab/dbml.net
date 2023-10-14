using System;

namespace DbmlNet.CodeAnalysis.Text;

/// <summary>
/// Represents a text span.
/// </summary>
public readonly struct TextSpan : IEquatable<TextSpan>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextSpan"/>.
    /// </summary>
    /// <param name="start">The start of the span.</param>
    /// <param name="length">The length of the span.</param>
    public TextSpan(int start, int length)
    {
        Start = start;
        Length = length;
    }

    /// <summary>
    /// Gets the start of the span.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// Gets the length of the span.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets the end of the span.
    /// </summary>
    public readonly int End => Start + Length;

    /// <summary>
    /// Creates a new <see cref="TextSpan"/> from the specified start and end bounds.
    /// </summary>
    /// <param name="start">The start position of the span.</param>
    /// <param name="end">The end position of the span.</param>
    /// <returns>A new <see cref="TextSpan"/> instance.</returns>
    public static TextSpan FromBounds(int start, int end)
    {
        int length = end - start;
        return new TextSpan(start, length);
    }

    /// <summary>
    /// Determines whether the current <see cref="TextSpan"/> overlaps with the specified <see cref="TextSpan"/>.
    /// </summary>
    /// <param name="span">The <see cref="TextSpan"/> to compare with the current <see cref="TextSpan"/>.</param>
    /// <returns>true if the <see cref="TextSpan"/>s overlap; otherwise, false.</returns>
    public bool OverlapsWith(TextSpan span)
    {
        return Start < span.End &&
               End > span.Start;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Start ^ End;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is TextSpan span && Equals(span);
    }

    /// <summary>
    /// Determines whether the current instance of <see cref="TextSpan"/> is equal to another <see cref="TextSpan"/>.
    /// </summary>
    /// <param name="other">The <see cref="TextSpan"/> to compare with the current instance.</param>
    /// <returns><c>true</c> if the current instance is equal to the other <see cref="TextSpan"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(TextSpan other)
    {
        return Start == other.Start && End == other.End;
    }

    /// <summary>
    /// Determines whether two instances of <see cref="TextSpan"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="TextSpan"/> to compare.</param>
    /// <param name="right">The second <see cref="TextSpan"/> to compare.</param>
    /// <returns><c>true</c> if the two instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(TextSpan left, TextSpan right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two instances of <see cref="TextSpan"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="TextSpan"/> to compare.</param>
    /// <param name="right">The second <see cref="TextSpan"/> to compare.</param>
    /// <returns><c>true</c> if the two instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(TextSpan left, TextSpan right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Returns a string representation of the <see cref="TextSpan"/>.
    /// </summary>
    /// <returns>A string representation of the <see cref="TextSpan"/>.</returns>
    public override string ToString() => $"{Start}..{End}";
}
