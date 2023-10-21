using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a global statement in the syntax tree.
/// </summary>
public sealed class GlobalStatementSyntax : MemberSyntax
{
    internal GlobalStatementSyntax(SyntaxTree syntaxTree, StatementSyntax statement)
        : base(syntaxTree)
    {
        Statement = statement;
    }

    /// <summary>
    /// Gets the syntax kind of the global statement <see cref="SyntaxKind.GlobalStatementMember"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.GlobalStatementMember;

    /// <summary>
    /// Gets the statement.
    /// </summary>
    public StatementSyntax Statement { get; }

    /// <summary>
    /// Gets the children of the global statement.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Statement;
    }
}
