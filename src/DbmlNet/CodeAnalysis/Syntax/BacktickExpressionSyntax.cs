using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a backtick expression in the syntax tree.
/// </summary>
public sealed class BacktickExpressionSyntax : ExpressionSyntax
{
    internal BacktickExpressionSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken openBacktickToken,
        ExpressionSyntax expression,
        SyntaxToken closeBacktickToken)
        : base(syntaxTree)
    {
        OpenBacktickToken = openBacktickToken;
        Expression = expression;
        CloseBacktickToken = closeBacktickToken;
    }

    /// <summary>
    /// Gets the syntax kind of the backtick expression <see cref="SyntaxKind.BacktickExpression"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.BacktickExpression;

    /// <summary>
    /// Gets the open backtick token.
    /// </summary>
    public SyntaxToken OpenBacktickToken { get; }

    /// <summary>
    /// Gets the expression.
    /// </summary>
    public ExpressionSyntax Expression { get; }

    /// <summary>
    /// Gets the close backtick token.
    /// </summary>
    public SyntaxToken CloseBacktickToken { get; }

    /// <summary>
    /// Gets the children of the backtick expression.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBacktickToken;
        yield return Expression;
        yield return CloseBacktickToken;
    }
}
