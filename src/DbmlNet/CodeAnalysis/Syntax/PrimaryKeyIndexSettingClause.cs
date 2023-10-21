using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a index setting clause in the syntax tree.
/// </summary>
public sealed class PrimaryKeyIndexSettingClause : IndexSettingClause
{
    internal PrimaryKeyIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken primaryKeyword,
        SyntaxToken keyKeyword)
        : base(syntaxTree)
    {
        PrimaryKeyword = primaryKeyword;
        KeyKeyword = keyKeyword;
        SettingName = primaryKeyword.Text + keyKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the primary key index setting clause <see cref="SyntaxKind.PrimaryKeyIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.PrimaryKeyIndexSettingClause;

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public override string SettingName { get; }

    /// <summary>
    /// Gets the primary keyword.
    /// </summary>
    public SyntaxToken PrimaryKeyword { get; }

    /// <summary>
    /// Gets the key keyword.
    /// </summary>
    public SyntaxToken KeyKeyword { get; }

    /// <summary>
    /// Gets the children of the primary key index setting.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return PrimaryKeyword;
        yield return KeyKeyword;
    }
}
