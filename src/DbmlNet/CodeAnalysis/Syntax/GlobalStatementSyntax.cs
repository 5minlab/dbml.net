using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class GlobalStatementSyntax : MemberSyntax
{
    internal GlobalStatementSyntax(SyntaxTree syntaxTree, StatementSyntax statement)
        : base(syntaxTree)
    {
        Statement = statement;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.GlobalStatementMember;

    /// <summary>
    /// </summary>
    public StatementSyntax Statement { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Statement;
    }
}
