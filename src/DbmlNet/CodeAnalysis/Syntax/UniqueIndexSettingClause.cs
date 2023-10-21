using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a unique index setting clause in the syntax tree.
/// </summary>
public sealed class UniqueIndexSettingClause : IndexSettingClause
{
    internal UniqueIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken uniqueKeyword)
        : base(syntaxTree)
    {
        UniqueKeyword = uniqueKeyword;
        SettingName = uniqueKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the unique index setting clause <see cref="SyntaxKind.UniqueIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UniqueIndexSettingClause;

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public override string SettingName { get; }

    /// <summary>
    /// Gets the unique keyword.
    /// </summary>
    public SyntaxToken UniqueKeyword { get; }

    /// <summary>
    /// Gets the children of the unique index setting.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return UniqueKeyword;
    }
}
