using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a trivia in the syntax tree.
/// </summary>
public sealed class SyntaxTrivia
{
    internal SyntaxTrivia(
        SyntaxTree syntaxTree, SyntaxKind kind, int position, string text)
    {
        SyntaxTree = syntaxTree;
        Kind = kind;
        Position = position;
        Text = text;
    }

    /// <summary>
    /// Gets the syntax tree containing the trivia.
    /// </summary>
    public SyntaxTree SyntaxTree { get; }

    /// <summary>
    /// Gets the kind of the trivia.
    /// </summary>
    public SyntaxKind Kind { get; }

    /// <summary>
    /// Gets the position of the trivia in the syntax tree.
    /// </summary>
    public int Position { get; }

    /// <summary>
    /// Gets the span of the trivia.
    /// </summary>
    public TextSpan Span => new(Position, Text?.Length ?? 0);

    /// <summary>
    /// Gets the text of the trivia.
    /// </summary>
    public string Text { get; }
}
