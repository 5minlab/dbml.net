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
        : base(syntaxTree, uniqueKeyword.Text)
    {
        UniqueKeyword = uniqueKeyword;
    }

    /// <summary>
    /// Gets the syntax kind of the unique index setting clause <see cref="SyntaxKind.UniqueIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UniqueIndexSettingClause;

    /// <summary>
    /// Gets the unique keyword.
    /// </summary>
    public SyntaxToken UniqueKeyword { get; }

    /// <summary>
    /// Gets the children of the unique index setting.
    /// </summary>
    /// <returns>The children of the unique index setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return UniqueKeyword;
    }
}
