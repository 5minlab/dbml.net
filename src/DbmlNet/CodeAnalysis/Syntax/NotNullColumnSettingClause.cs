using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a not null column setting clause in the syntax tree.
/// </summary>
public sealed class NotNullColumnSettingClause : ColumnSettingClause
{
    internal NotNullColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken notKeyword,
        SyntaxToken nullKeyword)
        : base(syntaxTree)
    {
        NotKeyword = notKeyword;
        NullKeyword = nullKeyword;
        SettingName = notKeyword.Text + nullKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the not null column setting clause <see cref="SyntaxKind.NotNullColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NotNullColumnSettingClause;

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public override string SettingName { get; }

    /// <summary>
    /// Gets the not keyword.
    /// </summary>
    public SyntaxToken NotKeyword { get; }

    /// <summary>
    /// Gets the null keyword.
    /// </summary>
    public SyntaxToken NullKeyword { get; }

    /// <summary>
    /// Gets the children of the not null column setting.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NotKeyword;
        yield return NullKeyword;
    }
}
