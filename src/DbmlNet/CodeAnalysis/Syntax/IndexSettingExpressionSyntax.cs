using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.IndexSettingExpression;

    /// <summary>
    /// </summary>
    public IEnumerable<SyntaxToken> Identifiers { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        foreach (SyntaxToken identifier in Identifiers)
            yield return identifier;
    }
}
