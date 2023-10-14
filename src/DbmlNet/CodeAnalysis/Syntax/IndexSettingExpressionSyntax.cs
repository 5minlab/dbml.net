using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a index setting clause in the syntax tree.
/// </summary>
public sealed class IndexSettingExpressionSyntax : ExpressionSyntax
{
    internal IndexSettingExpressionSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken[] identifiers)
        : base(syntaxTree)
    {
        Identifiers = identifiers;
    }

    /// <summary>
    /// Gets the syntax kind of the index setting expression <see cref="SyntaxKind.IndexSettingExpression"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.IndexSettingExpression;

    /// <summary>
    /// Gets the identifiers.
    /// </summary>
    public IEnumerable<SyntaxToken> Identifiers { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        foreach (SyntaxToken identifier in Identifiers)
            yield return identifier;
    }
}
