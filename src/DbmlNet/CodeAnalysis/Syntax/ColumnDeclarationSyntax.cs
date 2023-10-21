using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column declaration statement.
/// </summary>
public sealed class ColumnDeclarationSyntax : StatementSyntax
{
    internal ColumnDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken identifierToken,
        ColumnTypeClause columnTypeClause,
        ColumnSettingListSyntax? settingList)
        : base(syntaxTree)
    {
        IdentifierToken = identifierToken;
        ColumnTypeClause = columnTypeClause;
        SettingList = settingList;
    }

    /// <summary>
    /// Gets the syntax kind of the column declaration <see cref="SyntaxKind.ColumnDeclarationStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnDeclarationStatement;

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// Gets the column type.
    /// </summary>
    public ColumnTypeClause ColumnTypeClause { get; }

    /// <summary>
    /// Gets the column settings.
    /// </summary>
    public ColumnSettingListSyntax? SettingList { get; }

    /// <summary>
    /// Gets the children of the column declaration.
    /// </summary>
    /// <returns>The children of the column declaration.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
        yield return ColumnTypeClause;
        if (SettingList is not null) yield return SettingList;
    }
}
