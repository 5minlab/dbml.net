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
        : base(syntaxTree)
    {
        UniqueKeyword = uniqueKeyword;
        SettingName = uniqueKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the unique column setting clause <see cref="SyntaxKind.UniqueColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UniqueColumnSettingClause;

    /// <inherits/>
    public override string SettingName { get; }

    /// <summary>
    /// Gets the unique keyword.
    /// </summary>
    public SyntaxToken UniqueKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return UniqueKeyword;
    }
}
