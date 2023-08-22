using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class ExpressionStatementSyntax : StatementSyntax
{
    internal ExpressionStatementSyntax(SyntaxTree syntaxTree, ExpressionSyntax expression)
        : base(syntaxTree)
    {
        Expression = expression;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;

    /// <summary>
    /// </summary>
    public ExpressionSyntax Expression { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Expression;
    }
}
