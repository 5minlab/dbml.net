using System.Collections.Generic;
using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a long form relationship declaration statement in the syntax tree.
/// </summary>
public sealed class RelationshipLongFormDeclarationSyntax : RelationshipDeclarationSyntax
{
    internal RelationshipLongFormDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken refKeyword,
        SyntaxToken? identifierToken,
        SyntaxToken openBraceToken,
        ImmutableArray<RelationshipConstraintClause> relationships,
        SyntaxToken closeBraceToken)
        : base(syntaxTree)
    {
        RefKeyword = refKeyword;
        IdentifierToken = identifierToken;
        OpenBraceToken = openBraceToken;
        Relationships = relationships;
        CloseBraceToken = closeBraceToken;
    }

    /// <summary>
    /// Gets the syntax kind of the relationship declaration <see cref="SyntaxKind.RelationshipLongFormDeclarationMember"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.RelationshipLongFormDeclarationMember;

    /// <summary>
    /// Gets the ref keyword.
    /// </summary>
    public SyntaxToken RefKeyword { get; }

    /// <summary>
    /// Gets the relationship identifier token.
    /// </summary>
    public SyntaxToken? IdentifierToken { get; }

    /// <summary>
    /// Gets the open brace token.
    /// </summary>
    public SyntaxToken OpenBraceToken { get; }

    /// <summary>
    /// Gets the relationship constraints.
    /// </summary>
    public ImmutableArray<RelationshipConstraintClause> Relationships { get; }

    /// <summary>
    /// Gets the close brace token.
    /// </summary>
    public SyntaxToken CloseBraceToken { get; }

    /// <summary>
    /// Gets the children of the long form relationship declaration.
    /// </summary>
    /// <returns>The children of the long form relationship declaration.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return RefKeyword;
        if (IdentifierToken is not null) yield return IdentifierToken;
        yield return OpenBraceToken;

        foreach (RelationshipConstraintClause relationship in Relationships)
            yield return relationship;

        yield return CloseBraceToken;
    }
}
