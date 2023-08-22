using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnDeclarationStatement;

    /// <summary>
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// </summary>
    public ColumnTypeClause ColumnTypeClause { get; }

    /// <summary>
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
