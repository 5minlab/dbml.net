using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column type identifier clause in the syntax tree.
/// </summary>
public sealed class ColumnTypeIdentifierClause : ColumnTypeClause
{
    internal ColumnTypeIdentifierClause(SyntaxTree syntaxTree, SyntaxToken columnTypeToken)
        : base(syntaxTree)
    {
        ColumnTypeIdentifier = columnTypeToken;
    }

    /// <summary>
    /// Gets the syntax kind of the column type identifier clause <see cref="SyntaxKind.ColumnTypeIdentifierClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnTypeIdentifierClause;

    /// <summary>
    /// Gets the column type identifier.
    /// </summary>
    public SyntaxToken ColumnTypeIdentifier { get; }

    /// <summary>
    /// Gets the children of the column type identifier.
    /// </summary>
    /// <returns>The children of the column type identifier.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ColumnTypeIdentifier;
    }
}
