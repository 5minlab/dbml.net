using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a indexes declaration in the syntax tree.
/// </summary>
public sealed class IndexesDeclarationSyntax : StatementSyntax
{
    internal IndexesDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken indexesKeyword,
        SyntaxToken openBraceToken,
        SeparatedSyntaxList<IndexDeclarationStatementSyntax> indexes,
        SyntaxToken closeBraceToken)
        : base(syntaxTree)
    {
        IndexesKeyword = indexesKeyword;
        OpenBraceToken = openBraceToken;
        Indexes = indexes;
        CloseBraceToken = closeBraceToken;
    }

    /// <summary>
    /// Gets the syntax kind of the indexes declaration <see cref="SyntaxKind.IndexesDeclarationStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.IndexesDeclarationStatement;

    /// <summary>
    /// Gets the indexes keyword.
    /// </summary>
    public SyntaxToken IndexesKeyword { get; }

    /// <summary>
    /// Gets the open brace token.
    /// </summary>
    public SyntaxToken OpenBraceToken { get; }

    /// <summary>
    /// Gets the indexes.
    /// </summary>
    public SeparatedSyntaxList<IndexDeclarationStatementSyntax> Indexes { get; }

    /// <summary>
    /// Gets the close brace token.
    /// </summary>
    public SyntaxToken CloseBraceToken { get; }

    /// <summary>
    /// Gets the children of the indexes declaration.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IndexesKeyword;
        yield return OpenBraceToken;
        foreach (IndexDeclarationStatementSyntax index in Indexes)
            yield return index;
        yield return CloseBraceToken;
    }
}
