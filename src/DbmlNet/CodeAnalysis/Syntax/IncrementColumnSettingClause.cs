using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column setting clause in the syntax tree.
/// </summary>
public sealed class IncrementColumnSettingClause : ColumnSettingClause
{
    internal IncrementColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken incrementKeyword)
        : base(syntaxTree)
    {
        IncrementKeyword = incrementKeyword;
    }

    /// <summary>
    /// Gets the syntax kind of the increment column setting clause <see cref="SyntaxKind.IncrementColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.IncrementColumnSettingClause;

    /// <summary>
    /// Gets the increment keyword.
    /// </summary>
    public SyntaxToken IncrementKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IncrementKeyword;
    }
}
