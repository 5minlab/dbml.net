using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents an unknown index setting clause in the syntax tree.
/// </summary>
public sealed class UnknownIndexSettingClause : IndexSettingClause
{
    internal UnknownIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken nameToken,
        SyntaxToken? colonToken = null,
        SyntaxToken? settingValueToken = null)
        : base(syntaxTree)
    {
        NameToken = nameToken;
        ColonToken = colonToken;
        ValueToken = settingValueToken;
        SettingName = nameToken.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the unknown index setting clause <see cref="SyntaxKind.UnknownIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UnknownIndexSettingClause;

    /// <inherits/>
    public override string SettingName { get; }

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

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NameToken;
        if (ColonToken is not null) yield return ColonToken;
        if (ValueToken is not null) yield return ValueToken;
    }
}
