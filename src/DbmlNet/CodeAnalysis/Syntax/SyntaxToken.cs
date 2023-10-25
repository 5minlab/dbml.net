using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a token.
/// </summary>
public sealed class SyntaxToken : SyntaxNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxToken"/> class.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <param name="kind">The token kind.</param>
    /// <param name="start">The token start character position in the source text.</param>
    public SyntaxToken(
        SyntaxTree syntaxTree,
        SyntaxKind kind,
        int start)
        : this(
            syntaxTree, kind, start, text: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxToken"/> class.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <param name="kind">The token kind.</param>
    /// <param name="start">The token start character position in the source text.</param>
    /// <param name="text">The token text.</param>
    public SyntaxToken(
        SyntaxTree syntaxTree,
        SyntaxKind kind,
        int start,
        string? text)
        : this(syntaxTree, kind, start, text, value: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxToken"/> class.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <param name="kind">The token kind.</param>
    /// <param name="start">The token start character position in the source text.</param>
    /// <param name="text">The token text.</param>
    /// <param name="value">The value of the token.</param>
    public SyntaxToken(
        SyntaxTree syntaxTree,
        SyntaxKind kind,
        int start,
        string? text,
        object? value)
        : this(syntaxTree, kind, start, text, value, leadingTrivia: ImmutableArray<SyntaxTrivia>.Empty, trailingTrivia: ImmutableArray<SyntaxTrivia>.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxToken"/> class.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree.</param>
    /// <param name="kind">The token kind.</param>
    /// <param name="start">The token start character position in the source text.</param>
    /// <param name="text">The token text.</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="leadingTrivia">The leading trivia for the token.</param>
    /// <param name="trailingTrivia">The trailing trivia for the token.</param>
    public SyntaxToken(
        SyntaxTree syntaxTree,
        SyntaxKind kind,
        int start,
        string? text,
        object? value,
        ImmutableArray<SyntaxTrivia>? leadingTrivia,
        ImmutableArray<SyntaxTrivia>? trailingTrivia)
        : base(syntaxTree)
    {
        Kind = kind;
        Start = start;
        Text = text ?? string.Empty;
        IsMissing = text is null;
        Value = value;
        Length = Text.Length;
        End = Start + Length;
        LeadingTrivia = leadingTrivia ?? ImmutableArray<SyntaxTrivia>.Empty;
        TrailingTrivia = trailingTrivia ?? ImmutableArray<SyntaxTrivia>.Empty;
    }

    /// <summary>
    /// Gets the kind of this token.
    /// </summary>
    public override SyntaxKind Kind { get; }

    /// <summary>
    /// Gets the start character position in the source text for this token.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// Gets the text for this token.
    /// </summary>
    public override string Text { get; }

    /// <summary>
    /// Gets the length for this token.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets the end character position in the source text for this token.
    /// </summary>
    public int End { get; }

    /// <summary>
    /// Gets the value for this token.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets the leading trivia for this token.
    /// </summary>
    public ImmutableArray<SyntaxTrivia> LeadingTrivia { get; }

    /// <summary>
    /// Gets the trailing trivia for this token.
    /// </summary>
    public ImmutableArray<SyntaxTrivia> TrailingTrivia { get; }

    /// <summary>
    /// Gets a value indicating whether the token is missing, as it is the case
    /// with the inserted token by the parser which do not appear in source.
    /// </summary>
    public bool IsMissing { get; }

    /// <summary>
    /// Gets the span for this token.
    /// </summary>
    public override TextSpan Span => new(Start, Text.Length);

    /// <summary>
    /// Returns an empty array since a token has no children.
    /// </summary>
    /// <returns>An empty array.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Array.Empty<SyntaxNode>();
    }

    /// <summary>
    /// Returns the text for this token.
    /// </summary>
    /// <returns>The text for this token.</returns>
    public override string ToString() => Text;
}
