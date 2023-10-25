using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a table alias clause in the syntax tree.
/// </summary>
public sealed class TableAliasClause : SyntaxNode
{
    internal TableAliasClause(
        SyntaxTree syntaxTree,
        SyntaxToken asKeyword,
        SyntaxToken identifier)
        : base(syntaxTree)
    {
        AsKeyword = asKeyword;
        Identifier = identifier;
    }

    /// <summary>
    /// Gets the syntax kind of the table alias clause <see cref="SyntaxKind.TableAliasClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.TableAliasClause;

    /// <summary>
    /// Gets the as keyword.
    /// </summary>
    public SyntaxToken AsKeyword { get; }

    /// <summary>
    /// Gets the identifier of the table alias.
    /// </summary>
    public SyntaxToken Identifier { get; }

    /// <summary>
    /// Gets the children of the table alias clause.
    /// </summary>
    /// <returns>The children of the table alias clause.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return AsKeyword;
        yield return Identifier;
    }
}
