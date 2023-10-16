using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a table identifier clause in the syntax tree.
/// </summary>
public sealed class TableIdentifierClause : SyntaxNode
{
    internal TableIdentifierClause(
        SyntaxTree syntaxTree,
        SyntaxToken? databaseIdentifier,
        SyntaxToken? firstDotToken,
        SyntaxToken? schemaIdentifier,
        SyntaxToken? secondDotToken,
        SyntaxToken tableIdentifier)
        : base(syntaxTree)
    {
        DatabaseIdentifier = databaseIdentifier;
        FirstDotToken = firstDotToken;
        SchemaIdentifier = schemaIdentifier;
        SecondDotToken = secondDotToken;
        TableIdentifier = tableIdentifier;
    }

    /// <summary>
    /// Gets the syntax kind of the table identifier clause <see cref="SyntaxKind.TableIdentifierClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.TableIdentifierClause;

    /// <summary>
    /// Gets the schema identifier.
    /// </summary>
    public SyntaxToken? DatabaseIdentifier { get; }

    /// <summary>
    /// Gets the first dot token.
    /// </summary>
    public SyntaxToken? FirstDotToken { get; }

    /// <summary>
    /// Gets the table identifier.
    /// </summary>
    public SyntaxToken? SchemaIdentifier { get; }

    /// <summary>
    /// Gets the second dot token.
    /// </summary>
    public SyntaxToken? SecondDotToken { get; }

    /// <summary>
    /// Gets the table identifier.
    /// </summary>
    public SyntaxToken TableIdentifier { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        if (DatabaseIdentifier is not null) yield return DatabaseIdentifier;
        if (FirstDotToken is not null) yield return FirstDotToken;
        if (SchemaIdentifier is not null) yield return SchemaIdentifier;
        if (SecondDotToken is not null) yield return SecondDotToken;
        yield return TableIdentifier;
    }

    /// <summary>
    /// Returns the full text for this table identifier using format {database}.{schema}.{table}.
    /// </summary>
    /// <returns>The full text for this table identifier.</returns>
    public override string ToString()
    {
        return $"{DatabaseIdentifier?.Text}{FirstDotToken?.Text}{SchemaIdentifier?.Text}{SecondDotToken?.Text}{TableIdentifier.Text}";
    }
}
