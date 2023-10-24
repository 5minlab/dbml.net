using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column declaration statement.
/// </summary>
public sealed class EnumEntryDeclarationSyntax : StatementSyntax
{
    internal EnumEntryDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken identifierToken,
        EnumEntrySettingListSyntax? settingList)
        : base(syntaxTree)
    {
        IdentifierToken = identifierToken;
        SettingList = settingList;
    }

    /// <summary>
    /// Gets the syntax kind of the column declaration <see cref="SyntaxKind.EnumEntryDeclarationStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.EnumEntryDeclarationStatement;

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// Gets the column settings.
    /// </summary>
    public EnumEntrySettingListSyntax? SettingList { get; }

    /// <summary>
    /// Gets the children of the column declaration.
    /// </summary>
    /// <returns>The children of the column declaration.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
        if (SettingList is not null) yield return SettingList;
    }
}
