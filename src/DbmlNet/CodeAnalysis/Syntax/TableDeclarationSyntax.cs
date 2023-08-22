using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class TableDeclarationSyntax : MemberSyntax
{
    internal TableDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken tableKeyword,
        SyntaxToken identifierToken,
        StatementSyntax body)
        : base(syntaxTree)
    {
        TableKeyword = tableKeyword;
        IdentifierToken = identifierToken;
        Body = body;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.TableDeclarationMember;

    /// <summary>
    /// </summary>
    public SyntaxToken TableKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// </summary>
    public StatementSyntax Body { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return TableKeyword;
        yield return IdentifierToken;
        yield return Body;
    }
}
