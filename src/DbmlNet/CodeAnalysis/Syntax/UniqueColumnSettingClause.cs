using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a unique column setting clause in the syntax tree.
/// </summary>
public sealed class UniqueColumnSettingClause : ColumnSettingClause
{
    internal UniqueColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken uniqueKeyword)
        : base(syntaxTree, uniqueKeyword.Text)
    {
        UniqueKeyword = uniqueKeyword;
    }

    /// <summary>
    /// Gets the syntax kind of the unique column setting clause <see cref="SyntaxKind.UniqueColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UniqueColumnSettingClause;

    /// <summary>
    /// Gets the unique keyword.
    /// </summary>
    public SyntaxToken UniqueKeyword { get; }

    /// <summary>
    /// Gets the children of the unique column setting.
    /// </summary>
    /// <returns>The children of the unique column setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return UniqueKeyword;
    }
}
