using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnIdentifierClause;

    /// <summary>
    /// </summary>
    public SyntaxToken? SchemaIdentifier { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken? FirstDotToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken? TableIdentifier { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken? SecondDotToken { get; }

    /// <summary>
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

    /// <summary>
    /// Returns the full column identifier. E.g: {schema}.{table}.{column}.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{SchemaIdentifier?.Text}{FirstDotToken?.Text}{TableIdentifier?.Text}{SecondDotToken?.Text}{ColumnIdentifier.Text}";
    }
}
