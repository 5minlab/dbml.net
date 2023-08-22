using System;
using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a token.
/// </summary>
public sealed class SyntaxToken : SyntaxNode
{
    /// <summary>
    /// Initializes a new instance of <see cref="SyntaxToken"/>.
    /// </summary>
    /// <param name="syntaxTree"></param>
    /// <param name="kind">The token kind.</param>
    /// <param name="start">The token start character position in the source text.</param>
    /// <param name="text">The token text.</param>
    /// <param name="value">The value of the token.</param>
    public SyntaxToken(SyntaxTree syntaxTree, SyntaxKind kind, int start, string? text = null, object? value = null)
        : base(syntaxTree)
    {
        Kind = kind;
        Start = start;
        Text = text ?? string.Empty;
        IsMissing = text is null;
        Value = value;
        Length = Text.Length;
        End = Start + Length;
    }

    /// <summary>
    /// The kind of this token.
    /// </summary>
    public override SyntaxKind Kind { get; }

    /// <summary>
    /// The start character position in the source text for this token.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// The text for this token.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// The length for this token.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// The end character position in the source text for this token.
    /// </summary>
    public int End { get; }

    /// <summary>
    /// The value for this token.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// A token is missing if it was inserted by the parser and doesn't
    /// appear in source.
    /// </summary>
    public bool IsMissing { get; }

    /// <summary>
    /// </summary>
    public override TextSpan Span => new TextSpan(Start, Text.Length);

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Array.Empty<SyntaxNode>();
    }

    /// <summary>
    /// Returns the text for this token.
    /// </summary>
    public override string ToString() => Text;
}
