using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;

    /// <summary>
    /// </summary>
    public SyntaxToken OpenParenthesisToken { get; }

    /// <summary>
    /// </summary>
    public ExpressionSyntax Expression { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken CloseParenthesisToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenParenthesisToken;
        yield return Expression;
        yield return CloseParenthesisToken;
    }
}
