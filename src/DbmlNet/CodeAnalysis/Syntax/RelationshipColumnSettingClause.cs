using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a relationship column setting clause in the syntax tree.
/// </summary>
public sealed class RelationshipColumnSettingClause : ColumnSettingClause
{
    internal RelationshipColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken refKeyword,
        SyntaxToken colonToken,
        RelationshipConstraintClause constraintClause)
        : base(syntaxTree)
    {
        RefKeyword = refKeyword;
        ColonToken = colonToken;
        ConstraintClause = constraintClause;
    }

    /// <summary>
    /// Gets the syntax kind of the relationship column setting clause <see cref="SyntaxKind.RelationshipColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.RelationshipColumnSettingClause;

    /// <summary>
    /// Gets the ref keyword.
    /// </summary>
    public SyntaxToken RefKeyword { get; }

    /// <summary>
    /// Gets the colon token.
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// Gets the constraint clause.
    /// </summary>
    public RelationshipConstraintClause ConstraintClause { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return RefKeyword;
        yield return ColonToken;
        yield return ConstraintClause;
    }
}
