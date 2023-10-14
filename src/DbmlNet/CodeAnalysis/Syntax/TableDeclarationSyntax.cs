using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a table declaration statement in the syntax tree.
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
    /// Gets the syntax kind of the table declaration <see cref="SyntaxKind.TableDeclarationMember"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.TableDeclarationMember;

    /// <summary>
    /// Gets the table keyword.
    /// </summary>
    public SyntaxToken TableKeyword { get; }

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// Gets the body.
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
