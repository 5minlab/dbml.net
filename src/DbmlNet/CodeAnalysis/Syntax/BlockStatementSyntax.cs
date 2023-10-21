using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a block statement in the syntax tree.
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
    /// Gets the syntax kind of the block statement <see cref="SyntaxKind.BlockStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.BlockStatement;

    /// <summary>
    /// Gets the open brace token.
    /// </summary>
    public SyntaxToken OpenBraceToken { get; }

    /// <summary>
    /// Description of the Statements property.
    /// </summary>
    public IEnumerable<StatementSyntax> Statements { get; }

    /// <summary>
    /// Gets the close brace token.
    /// </summary>
    public SyntaxToken CloseBraceToken { get; }

    /// <summary>
    /// Gets the children of the block statement.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenBraceToken;
        foreach (StatementSyntax statement in Statements)
            yield return statement;
        yield return CloseBraceToken;
    }
}
