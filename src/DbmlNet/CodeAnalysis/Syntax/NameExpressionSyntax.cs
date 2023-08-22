using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class NameExpressionSyntax : ExpressionSyntax
{
    internal NameExpressionSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken identifierToken)
        : base(syntaxTree)
    {
        IdentifierToken = identifierToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NameExpression;

    /// <summary>
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
    }
}
