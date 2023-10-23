using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a short form relationship declaration statement in the syntax tree.
/// </summary>
public sealed class RelationshipShortFormDeclarationSyntax : RelationshipDeclarationSyntax
{
    internal RelationshipShortFormDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken refKeyword,
        SyntaxToken? identifierToken,
        SyntaxToken colonToken,
        RelationshipConstraintClause relationship)
        : base(syntaxTree)
    {
        RefKeyword = refKeyword;
        IdentifierToken = identifierToken;
        ColonToken = colonToken;
        Relationship = relationship;
    }

    /// <summary>
    /// Gets the syntax kind of the relationship declaration <see cref="SyntaxKind.RelationshipShortFormDeclarationMember"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.RelationshipShortFormDeclarationMember;

    /// <summary>
    /// Gets the ref keyword.
    /// </summary>
    public SyntaxToken RefKeyword { get; }

    /// <summary>
    /// Gets the relationship identifier token.
    /// </summary>
    public SyntaxToken? IdentifierToken { get; }

    /// <summary>
    /// Gets the colon token.
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// Gets the relationship constraint.
    /// </summary>
    public RelationshipConstraintClause Relationship { get; }

    /// <summary>
    /// Gets the children of the short form relationship declaration.
    /// </summary>
    /// <returns>The children of the short form relationship declaration.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return RefKeyword;
        if (IdentifierToken is not null) yield return IdentifierToken;
        yield return ColonToken;
        yield return Relationship;
    }
}
