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
    /// Initializes a new instance of <see cref="SyntaxToken"/>.
    /// </summary>
    /// <param name="syntaxTree"></param>
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
    /// Initializes a new instance of <see cref="SyntaxToken"/>.
    /// </summary>
    /// <param name="syntaxTree"></param>
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
    /// Initializes a new instance of <see cref="SyntaxToken"/>.
    /// </summary>
    /// <param name="syntaxTree"></param>
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
        : this(
            syntaxTree, kind, start, text, value,
            leadingTrivia: ImmutableArray<SyntaxTrivia>.Empty,
            trailingTrivia: ImmutableArray<SyntaxTrivia>.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SyntaxToken"/>.
    /// </summary>
    /// <param name="syntaxTree"></param>
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
    public override string Text { get; }

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
    /// The leading trivia for this token.
    /// </summary>
    public ImmutableArray<SyntaxTrivia> LeadingTrivia { get; }

    /// <summary>
    /// The trailing trivia for this token.
    /// </summary>
    public ImmutableArray<SyntaxTrivia> TrailingTrivia { get; }

    /// <summary>
    /// A token is missing if it was inserted by the parser and doesn't
    /// appear in source.
    /// </summary>
    public bool IsMissing { get; }

    /// <summary>
    /// The span for this token.
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
