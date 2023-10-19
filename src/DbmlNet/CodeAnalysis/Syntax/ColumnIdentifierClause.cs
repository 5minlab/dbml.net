using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column identifier clause in the syntax tree.
/// </summary>
public sealed class ColumnIdentifierClause : SyntaxNode
{
    internal ColumnIdentifierClause(
        SyntaxTree syntaxTree,
        SyntaxToken? schemaIdentifier,
        SyntaxToken? firstDotToken,
        SyntaxToken? tableIdentifier,
        SyntaxToken? secondDotToken,
        SyntaxToken columnIdentifier)
        : base(syntaxTree)
    {
        SchemaIdentifier = schemaIdentifier;
        FirstDotToken = firstDotToken;
        TableIdentifier = tableIdentifier;
        SecondDotToken = secondDotToken;
        ColumnIdentifier = columnIdentifier;
    }

    /// <summary>
    /// Gets the syntax kind of the column identifier clause <see cref="SyntaxKind.ColumnIdentifierClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnIdentifierClause;

    /// <summary>
    /// Gets the schema identifier.
    /// </summary>
    public SyntaxToken? SchemaIdentifier { get; }

    /// <summary>
    /// Gets the first dot token.
    /// </summary>
    public SyntaxToken? FirstDotToken { get; }

    /// <summary>
    /// Gets the table identifier.
    /// </summary>
    public SyntaxToken? TableIdentifier { get; }

    /// <summary>
    /// Gets the second dot token.
    /// </summary>
    public SyntaxToken? SecondDotToken { get; }

    /// <summary>
    /// Gets the column identifier.
    /// </summary>
    public SyntaxToken ColumnIdentifier { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        if (SchemaIdentifier is not null) yield return SchemaIdentifier;
        if (FirstDotToken is not null) yield return FirstDotToken;
        if (TableIdentifier is not null) yield return TableIdentifier;
        if (SecondDotToken is not null) yield return SecondDotToken;
        yield return ColumnIdentifier;
    }
}
