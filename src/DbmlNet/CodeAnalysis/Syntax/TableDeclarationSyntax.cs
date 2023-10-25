using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a table declaration statement in the syntax tree.
/// </summary>
public sealed class TableDeclarationSyntax : MemberSyntax
{
    internal TableDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken tableKeyword,
        TableIdentifierClause identifierToken,
        TableAliasClause? alias,
        TableSettingListSyntax? settings,
        StatementSyntax body)
        : base(syntaxTree)
    {
        TableKeyword = tableKeyword;
        DbSchema = identifierToken;
        Alias = alias;
        Settings = settings;
        Body = body;
    }

    /// <summary>
    /// Gets the syntax kind of the table declaration <see cref="SyntaxKind.TableDeclarationMember"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.TableDeclarationMember;

    /// <summary>
    /// Gets the table keyword.
    /// </summary>
    public SyntaxToken TableKeyword { get; }

    /// <summary>
    /// Gets the full table identifier (e.g: database.schema.table).
    /// </summary>
    public TableIdentifierClause DbSchema { get; }

    /// <summary>
    /// Gets the table alias.
    /// </summary>
    public TableAliasClause? Alias { get; }

    /// <summary>
    /// Gets the table setting list.
    /// </summary>
    public TableSettingListSyntax? Settings { get; }

    /// <summary>
    /// Gets the body.
    /// </summary>
    public StatementSyntax Body { get; }

    /// <summary>
    /// Gets the children of the table declaration.
    /// </summary>
    /// <returns>The children of the table declaration.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return TableKeyword;
        yield return DbSchema;
        if (Alias is not null) yield return Alias;
        if (Settings is not null) yield return Settings;
        yield return Body;
    }
}
