using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column setting clause in the syntax tree.
/// </summary>
public sealed class PkColumnSettingClause : ColumnSettingClause
{
    internal PkColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken pkKeyword)
        : base(syntaxTree)
    {
        PkKeyword = pkKeyword;
        SettingName = pkKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the pk column setting clause <see cref="SyntaxKind.PkColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.PkColumnSettingClause;

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public override string SettingName { get; }

    /// <summary>
    /// Gets the pk keyword.
    /// </summary>
    public SyntaxToken PkKeyword { get; }

    /// <summary>
    /// Gets the children of the pk column setting.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return PkKeyword;
    }
}
