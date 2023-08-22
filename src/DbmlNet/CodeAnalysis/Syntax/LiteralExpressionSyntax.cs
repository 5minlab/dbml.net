using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class LiteralExpressionSyntax : ExpressionSyntax
{
    internal LiteralExpressionSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken literalToken)
        : this(syntaxTree, literalToken, literalToken.Value!)
    {
    }

    internal LiteralExpressionSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken literalToken,
        object value)
        : base(syntaxTree)
    {
        LiteralToken = literalToken;
        Value = value;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

    /// <summary>
    /// </summary>
    public SyntaxToken LiteralToken { get; }

    /// <summary>
    /// </summary>
    public object Value { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return LiteralToken;
    }
}
