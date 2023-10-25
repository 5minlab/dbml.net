using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a enum declaration in the syntax tree.
/// </summary>
public sealed class EnumDeclarationSyntax : MemberSyntax
{
    internal EnumDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken enumKeyword,
        EnumIdentifierClause identifier,
        StatementSyntax body)
        : base(syntaxTree)
    {
        EnumKeyword = enumKeyword;
        Identifier = identifier;
        Body = body;
    }

    /// <summary>
    /// Gets the syntax kind of the enum declaration <see cref="SyntaxKind.EnumDeclarationMember"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.EnumDeclarationMember;

    /// <summary>
    /// Gets the enum keyword.
    /// </summary>
    public SyntaxToken EnumKeyword { get; }

    /// <summary>
    /// Gets the full enum identifier (e.g: schema.enum).
    /// </summary>
    public EnumIdentifierClause Identifier { get; }

    /// <summary>
    /// Gets the body.
    /// </summary>
    public StatementSyntax Body { get; }

    /// <summary>
    /// Gets the children of the enum declaration.
    /// </summary>
    /// <returns>The children of the enum declaration.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return EnumKeyword;
        yield return Identifier;
        yield return Body;
    }
}
