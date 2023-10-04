using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.IndexSettingListClause;

    /// <summary>
    /// </summary>
    public SyntaxToken OpenBracketToken { get; }

    /// <summary>
    /// </summary>
    public SeparatedSyntaxList<IndexSettingClause> Settings { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken CloseBracketToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBracketToken;
        foreach (SyntaxNode setting in Settings.GetWithSeparators())
            yield return setting;
        yield return CloseBracketToken;
    }
}
