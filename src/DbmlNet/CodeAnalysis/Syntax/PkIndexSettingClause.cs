using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a index setting clause in the syntax tree.
/// </summary>
public sealed class PkIndexSettingClause : IndexSettingClause
{
    internal PkIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken pkKeyword)
        : base(syntaxTree)
    {
        PkKeyword = pkKeyword;
        SettingName = pkKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the index setting clause <see cref="SyntaxKind.PkIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.PkIndexSettingClause;

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
    /// <returns>The children of the pk column setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return PkKeyword;
    }
}
