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
        : base(syntaxTree, pkKeyword.Text)
    {
        PkKeyword = pkKeyword;
    }

    /// <summary>
    /// Gets the syntax kind of the pk column setting clause <see cref="SyntaxKind.PkColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.PkColumnSettingClause;

    /// <summary>
    /// Gets the pk keyword.
    /// </summary>
    public SyntaxToken PkKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return PkKeyword;
    }
}
