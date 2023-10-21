using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a literal expression in the syntax tree.
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
    /// Gets the syntax kind of the literal expression <see cref="SyntaxKind.LiteralExpression"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

    /// <summary>
    /// Gets the literal token.
    /// </summary>
    public SyntaxToken LiteralToken { get; }

    /// <summary>
    /// Gets the literal value.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Gets the children of the literal expression.
    /// </summary>
    /// <returns>The children of the literal expression.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return LiteralToken;
    }
}
