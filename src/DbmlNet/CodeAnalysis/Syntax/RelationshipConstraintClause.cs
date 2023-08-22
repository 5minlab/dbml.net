using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.RelationshipConstraintClause;

    /// <summary>
    /// </summary>
    public ColumnIdentifierClause? FromIdentifier { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken RelationshipTypeToken { get; }

    /// <summary>
    /// </summary>
    public ColumnIdentifierClause ToIdentifier { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        if (FromIdentifier is not null) yield return FromIdentifier;
        yield return RelationshipTypeToken;
        yield return ToIdentifier;
    }
}
