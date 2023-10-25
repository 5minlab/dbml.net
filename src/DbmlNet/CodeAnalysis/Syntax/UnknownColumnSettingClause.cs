using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column setting clause in the syntax tree.
/// </summary>
public sealed class UnknownColumnSettingClause : ColumnSettingClause
{
    internal UnknownColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken settingNameToken,
        SyntaxToken? colonToken = null,
        SyntaxToken? settingValueToken = null)
        : base(syntaxTree, settingNameToken.Text)
    {
        NameToken = settingNameToken;
        ColonToken = colonToken;
        ValueToken = settingValueToken;
    }

    /// <summary>
    /// Gets the syntax kind of the unknown column setting clause <see cref="SyntaxKind.UnknownColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UnknownColumnSettingClause;

    /// <summary>
    /// Gets the name token.
    /// </summary>
    public SyntaxToken NameToken { get; }

    /// <summary>
    /// Gets the colon token.
    /// </summary>
    public SyntaxToken? ColonToken { get; }

    /// <summary>
    /// Gets the value token.
    /// </summary>
    public SyntaxToken? ValueToken { get; }

    /// <summary>
    /// Gets the children of the unknown column setting.
    /// </summary>
    /// <returns>The children of the unknown column setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NameToken;
        if (ColonToken is not null) yield return ColonToken;
        if (ValueToken is not null) yield return ValueToken;
    }
}
