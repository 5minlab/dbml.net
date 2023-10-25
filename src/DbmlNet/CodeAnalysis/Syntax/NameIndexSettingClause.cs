using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a index setting clause in the syntax tree.
/// </summary>
public sealed class NameIndexSettingClause : IndexSettingClause
{
    internal NameIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken nameKeyword,
        SyntaxToken colonToken,
        SyntaxToken valueToken)
        : base(syntaxTree, nameKeyword.Text)
    {
        NameKeyword = nameKeyword;
        ColonToken = colonToken;
        ValueToken = valueToken;
    }

    /// <summary>
    /// Gets the syntax kind of the index setting clause <see cref="SyntaxKind.NameIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NameIndexSettingClause;

    /// <summary>
    /// Gets the name keyword.
    /// </summary>
    public SyntaxToken NameKeyword { get; }

    /// <summary>
    /// Gets the colon token.
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// Gets the value token.
    /// </summary>
    public SyntaxToken ValueToken { get; }

    /// <summary>
    /// Gets the children of the index setting.
    /// </summary>
    /// <returns>The children of the index setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NameKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
