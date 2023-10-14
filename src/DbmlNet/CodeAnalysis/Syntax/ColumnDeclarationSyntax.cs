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
    /// Gets the syntax kind of the column declaration statement <see cref="SyntaxKind.ColumnDeclarationStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnDeclarationStatement;

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// Gets the column type clause.
    /// </summary>
    public ColumnTypeClause ColumnTypeClause { get; }

    /// <summary>
    /// Gets the column setting list clause.
    /// </summary>
    public ColumnSettingListSyntax? SettingList { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
        yield return ColumnTypeClause;
        if (SettingList is not null) yield return SettingList;
    }
}
