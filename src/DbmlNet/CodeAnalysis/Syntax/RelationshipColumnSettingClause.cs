using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.RelationshipColumnSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken RefKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
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
