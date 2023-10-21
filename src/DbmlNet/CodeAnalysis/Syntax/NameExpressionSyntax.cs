using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a name expression in the syntax tree.
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
    /// Gets the syntax kind of the name expression <see cref="SyntaxKind.NameExpression"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NameExpression;

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// Gets the children of the name expression.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
    }
}
