using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a relationship constraint clause in the syntax tree.
/// </summary>
public sealed class RelationshipConstraintClause : SyntaxNode
{
    internal RelationshipConstraintClause(
        SyntaxTree syntaxTree,
        ColumnIdentifierClause? fromIdentifier,
        SyntaxToken relationshipTypeToken,
        ColumnIdentifierClause toIdentifier)
        : base(syntaxTree)
    {
        FromIdentifier = fromIdentifier;
        RelationshipTypeToken = relationshipTypeToken;
        ToIdentifier = toIdentifier;
    }

    /// <summary>
    /// Gets the syntax kind of the relationship constraint clause <see cref="SyntaxKind.RelationshipConstraintClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.RelationshipConstraintClause;

    /// <summary>
    /// Gets the from identifier.
    /// </summary>
    public ColumnIdentifierClause? FromIdentifier { get; }

    /// <summary>
    /// Gets the relationship type token.
    /// </summary>
    public SyntaxToken RelationshipTypeToken { get; }

    /// <summary>
    /// Gets the to identifier.
    /// </summary>
    public ColumnIdentifierClause ToIdentifier { get; }

    /// <summary>
    /// Gets the children of the relationship constraint.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        if (FromIdentifier is not null) yield return FromIdentifier;
        yield return RelationshipTypeToken;
        yield return ToIdentifier;
    }
}
