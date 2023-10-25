using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column setting clause in the syntax tree.
/// </summary>
public sealed class NullColumnSettingClause : ColumnSettingClause
{
    internal NullColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken nullKeyword)
        : base(syntaxTree, nullKeyword.Text)
    {
        NullKeyword = nullKeyword;
    }

    /// <summary>
    /// Gets the syntax kind of the null column setting clause <see cref="SyntaxKind.NullColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NullColumnSettingClause;

    /// <summary>
    /// Gets the null keyword.
    /// </summary>
    public SyntaxToken NullKeyword { get; }

    /// <summary>
    /// Gets the children of the null column setting.
    /// </summary>
    /// <returns>The children of the null column setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NullKeyword;
    }
}
