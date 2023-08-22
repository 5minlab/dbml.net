using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class BlockStatementSyntax : StatementSyntax
{
    internal BlockStatementSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken openBraceToken,
        StatementSyntax[] statements,
        SyntaxToken closeBraceToken)
        : base(syntaxTree)
    {
        OpenBraceToken = openBraceToken;
        Statements = statements;
        CloseBraceToken = closeBraceToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.BlockStatement;

    /// <summary>
    /// </summary>
    public SyntaxToken OpenBraceToken { get; }

    /// <summary>
    /// </summary>
    public IEnumerable<StatementSyntax> Statements { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken CloseBraceToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBraceToken;
        foreach (StatementSyntax statement in Statements)
            yield return statement;
        yield return CloseBraceToken;
    }
}
