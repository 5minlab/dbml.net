using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a null expression in the syntax tree.
/// </summary>
public sealed class NullExpressionSyntax : ExpressionSyntax
{
    internal NullExpressionSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken nullToken)
        : base(syntaxTree)
    {
        NullToken = nullToken;
    }

    /// <summary>
    /// Gets the syntax kind of the null expression <see cref="SyntaxKind.NullExpression"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NullExpression;

    /// <summary>
    /// Gets the null token.
    /// </summary>
    public SyntaxToken NullToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NullToken;
    }
}
