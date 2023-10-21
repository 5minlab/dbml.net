using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a list of index settings in the syntax tree.
/// </summary>
public sealed class IndexSettingListSyntax : SyntaxNode
{
    internal IndexSettingListSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken openBracketToken,
        SeparatedSyntaxList<IndexSettingClause> settings,
        SyntaxToken closeBracketToken)
        : base(syntaxTree)
    {
        OpenBracketToken = openBracketToken;
        Settings = settings;
        CloseBracketToken = closeBracketToken;
    }

    /// <summary>
    /// Gets the syntax kind of the index setting list clause <see cref="SyntaxKind.IndexSettingListClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.IndexSettingListClause;

    /// <summary>
    /// Gets the open bracket token.
    /// </summary>
    public SyntaxToken OpenBracketToken { get; }

    /// <summary>
    /// Gets the index settings.
    /// </summary>
    public SeparatedSyntaxList<IndexSettingClause> Settings { get; }

    /// <summary>
    /// Gets the close bracket token.
    /// </summary>
    public SyntaxToken CloseBracketToken { get; }

    /// <summary>
    /// Gets the children of the index setting list.
    /// </summary>
    /// <returns>The children of the index setting list.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBracketToken;
        foreach (SyntaxNode setting in Settings.GetWithSeparators())
            yield return setting;
        yield return CloseBracketToken;
    }
}
