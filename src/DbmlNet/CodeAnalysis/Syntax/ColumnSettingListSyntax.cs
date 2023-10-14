using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column setting list clause in the syntax tree.
/// </summary>
public sealed class ColumnSettingListSyntax : SyntaxNode
{
    internal ColumnSettingListSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken openBracketToken,
        SeparatedSyntaxList<ColumnSettingClause> settings,
        SyntaxToken closeBracketToken)
        : base(syntaxTree)
    {
        OpenBracketToken = openBracketToken;
        Settings = settings;
        CloseBracketToken = closeBracketToken;
    }

    /// <summary>
    /// Gets the syntax kind of the column setting list clause <see cref="SyntaxKind.ColumnSettingListClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnSettingListClause;

    /// <summary>
    /// Gets the open bracket token.
    /// </summary>
    public SyntaxToken OpenBracketToken { get; }

    /// <summary>
    /// Gets the column setting list.
    /// </summary>
    public SeparatedSyntaxList<ColumnSettingClause> Settings { get; }

    /// <summary>
    /// Gets the close bracket token.
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
