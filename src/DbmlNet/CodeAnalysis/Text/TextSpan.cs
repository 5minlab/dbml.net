using System;

namespace DbmlNet.CodeAnalysis.Text;

/// <summary>
/// </summary>
public readonly struct TextSpan : IEquatable<TextSpan>
{
    /// <summary>
    /// </summary>
    /// <param name="start"></param>
    /// <param name="length"></param>
    public TextSpan(int start, int length)
    {
        Start = start;
        Length = length;
    }

    /// <summary>
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// </summary>
    public readonly int End => Start + Length;

    /// <summary>
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static TextSpan FromBounds(int start, int end)
    {
        int length = end - start;
        return new TextSpan(start, length);
    }

    /// <summary>
    /// </summary>
    /// <param name="span"></param>
    /// <returns></returns>
    public bool OverlapsWith(TextSpan span)
    {
        return Start < span.End &&
               End > span.Start;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return Start ^ End;
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return obj is TextSpan span && Equals(span);
    }

    /// <summary>
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(TextSpan other)
    {
        return Start == other.Start && End == other.End;
    }

    /// <summary>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(TextSpan left, TextSpan right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(TextSpan left, TextSpan right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Start}..{End}";
}
