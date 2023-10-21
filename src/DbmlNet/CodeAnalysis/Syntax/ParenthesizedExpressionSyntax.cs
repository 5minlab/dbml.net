using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a parenthesized expression in the syntax tree.
/// </summary>
public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
{
    internal ParenthesizedExpressionSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken openParenthesisToken,
        ExpressionSyntax expression,
        SyntaxToken closeParenthesisToken)
        : base(syntaxTree)
    {
        OpenParenthesisToken = openParenthesisToken;
        Expression = expression;
        CloseParenthesisToken = closeParenthesisToken;
    }

    /// <summary>
    /// Gets the syntax kind of the parenthesized expression <see cref="SyntaxKind.ParenthesizedExpression"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;

    /// <summary>
    /// Gets the open parenthesis token.
    /// </summary>
    public SyntaxToken OpenParenthesisToken { get; }

    /// <summary>
    /// Gets the expression.
    /// </summary>
    public ExpressionSyntax Expression { get; }

    /// <summary>
    /// Gets the close parenthesis token.
    /// </summary>
    public SyntaxToken CloseParenthesisToken { get; }

    /// <summary>
    /// Gets the children of the parenthesized expression.
    /// </summary>
    /// <returns>The children of the parenthesized expression.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenParenthesisToken;
        yield return Expression;
        yield return CloseParenthesisToken;
    }
}
