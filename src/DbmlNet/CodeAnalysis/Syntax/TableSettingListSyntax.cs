using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a list of table settings in the syntax tree.
/// </summary>
public sealed class TableSettingListSyntax : SyntaxNode
{
    internal TableSettingListSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken openBracketToken,
        SeparatedSyntaxList<TableSettingClause> settings,
        SyntaxToken closeBracketToken)
        : base(syntaxTree)
    {
        OpenBracketToken = openBracketToken;
        Settings = settings;
        CloseBracketToken = closeBracketToken;
    }

    /// <summary>
    /// Gets the syntax kind of the table setting list clause <see cref="SyntaxKind.TableSettingListClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.TableSettingListClause;

    /// <summary>
    /// Gets the open bracket token.
    /// </summary>
    public SyntaxToken OpenBracketToken { get; }

    /// <summary>
    /// Gets the table setting list.
    /// </summary>
    public SeparatedSyntaxList<TableSettingClause> Settings { get; }

    /// <summary>
    /// Gets the close bracket token.
    /// </summary>
    public SyntaxToken CloseBracketToken { get; }

    /// <summary>
    /// Gets the children of the table setting list.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBracketToken;
        foreach (SyntaxNode setting in Settings.GetWithSeparators())
            yield return setting;
        yield return CloseBracketToken;
    }
}
