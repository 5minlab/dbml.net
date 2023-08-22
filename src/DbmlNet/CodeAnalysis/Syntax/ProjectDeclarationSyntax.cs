using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class ProjectDeclarationSyntax : MemberSyntax
{
    internal ProjectDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken projectKeyword,
        SyntaxToken identifierToken,
        StatementSyntax body)
        : base(syntaxTree)
    {
        ProjectKeyword = projectKeyword;
        IdentifierToken = identifierToken;
        Body = body;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ProjectDeclarationMember;

    /// <summary>
    /// </summary>
    public SyntaxToken ProjectKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// </summary>
    public StatementSyntax Body { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ProjectKeyword;
        yield return IdentifierToken;
        yield return Body;
    }
}
