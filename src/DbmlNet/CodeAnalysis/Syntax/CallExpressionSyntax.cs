using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a call expression in the syntax tree.
/// </summary>
public sealed class CallExpressionSyntax : ExpressionSyntax
{
    internal CallExpressionSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken identifier,
        SyntaxToken openParenthesisToken,
        SeparatedSyntaxList<ExpressionSyntax> arguments,
        SyntaxToken closeParenthesisToken)
        : base(syntaxTree)
    {
        Identifier = identifier;
        OpenParenthesisToken = openParenthesisToken;
        Arguments = arguments;
        CloseParenthesisToken = closeParenthesisToken;
    }

    /// <summary>
    /// Gets the syntax kind of the call expression <see cref="SyntaxKind.CallExpression"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.CallExpression;

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken Identifier { get; }

    /// <summary>
    /// Gets the open parenthesis token.
    /// </summary>
    public SyntaxToken OpenParenthesisToken { get; }

    /// <summary>
    /// Gets the arguments.
    /// </summary>
    public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }

    /// <summary>
    /// Gets the close parenthesis token.
    /// </summary>
    public SyntaxToken CloseParenthesisToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Identifier;
        yield return OpenParenthesisToken;

        foreach (ExpressionSyntax argument in Arguments)
            yield return argument;

        yield return CloseParenthesisToken;
    }

}
