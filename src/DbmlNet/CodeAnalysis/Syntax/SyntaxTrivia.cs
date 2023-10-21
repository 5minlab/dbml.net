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
    /// The syntax tree containing the trivia.
    /// </summary>
    public SyntaxTree SyntaxTree { get; }

    /// <summary>
    /// The kind of the trivia.
    /// </summary>
    public SyntaxKind Kind { get; }

    /// <summary>
    /// The position of the trivia in the syntax tree.
    /// </summary>
    public int Position { get; }

    /// <summary>
    /// The span of the trivia.
    /// </summary>
    public TextSpan Span => new(Position, Text?.Length ?? 0);

    /// <summary>
    /// The text of the trivia.
    /// </summary>
    public string Text { get; }
}
