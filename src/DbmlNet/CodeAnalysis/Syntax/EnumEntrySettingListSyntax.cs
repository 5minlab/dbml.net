using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a enum entry setting list clause in the syntax tree.
/// </summary>
public sealed class EnumEntrySettingListSyntax : SyntaxNode
{
    internal EnumEntrySettingListSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken openBracketToken,
        SeparatedSyntaxList<EnumEntrySettingClause> settings,
        SyntaxToken closeBracketToken)
        : base(syntaxTree)
    {
        OpenBracketToken = openBracketToken;
        Settings = settings;
        CloseBracketToken = closeBracketToken;
    }

    /// <summary>
    /// Gets the syntax kind of the enum entry setting list clause <see cref="SyntaxKind.EnumEntrySettingListClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.EnumEntrySettingListClause;

    /// <summary>
    /// Gets the open bracket token.
    /// </summary>
    public SyntaxToken OpenBracketToken { get; }

    /// <summary>
    /// Gets the enum entry settings.
    /// </summary>
    public SeparatedSyntaxList<EnumEntrySettingClause> Settings { get; }

    /// <summary>
    /// Gets the close bracket token.
    /// </summary>
    public SyntaxToken CloseBracketToken { get; }

    /// <summary>
    /// Gets the children of the enum entry setting list.
    /// </summary>
    /// <returns>The children of the enum entry setting list.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBracketToken;
        foreach (SyntaxNode setting in Settings.GetWithSeparators())
            yield return setting;
        yield return CloseBracketToken;
    }
}
