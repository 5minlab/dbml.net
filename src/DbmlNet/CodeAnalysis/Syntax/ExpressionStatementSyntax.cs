using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents an expression statement in the syntax tree.
/// </summary>
public sealed class ExpressionStatementSyntax : StatementSyntax
{
    internal ExpressionStatementSyntax(SyntaxTree syntaxTree, ExpressionSyntax expression)
        : base(syntaxTree)
    {
        Expression = expression;
    }

    /// <summary>
    /// Gets the syntax kind of the expression statement <see cref="SyntaxKind.ExpressionStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;

    /// <summary>
    /// Gets the expression.
    /// </summary>
    public ExpressionSyntax Expression { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Expression;
    }
}
