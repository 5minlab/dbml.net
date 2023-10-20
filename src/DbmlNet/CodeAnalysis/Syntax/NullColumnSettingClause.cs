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
        : base(syntaxTree)
    {
        NullKeyword = nullKeyword;
        SettingName = nullKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the null column setting clause <see cref="SyntaxKind.NullColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NullColumnSettingClause;

    /// <inherits/>
    public override string SettingName { get; }

    /// <summary>
    /// Gets the null keyword.
    /// </summary>
    public SyntaxToken NullKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NullKeyword;
    }
}
