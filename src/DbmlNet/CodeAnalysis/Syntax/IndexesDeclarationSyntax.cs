using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class IndexesDeclarationSyntax : StatementSyntax
{
    internal IndexesDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken indexesKeyword,
        SyntaxToken openBraceToken,
        SeparatedSyntaxList<StatementSyntax> indexes,
        SyntaxToken closeBraceToken)
        : base(syntaxTree)
    {
        IndexesKeyword = indexesKeyword;
        OpenBraceToken = openBraceToken;
        Indexes = indexes;
        CloseBraceToken = closeBraceToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.IndexesDeclarationStatement;

    /// <summary>
    /// </summary>
    public SyntaxToken IndexesKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken OpenBraceToken { get; }

    /// <summary>
    /// </summary>
    public SeparatedSyntaxList<StatementSyntax> Indexes { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken CloseBraceToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IndexesKeyword;
        yield return OpenBraceToken;
        foreach (StatementSyntax index in Indexes)
            yield return index;
        yield return CloseBraceToken;
    }
}
