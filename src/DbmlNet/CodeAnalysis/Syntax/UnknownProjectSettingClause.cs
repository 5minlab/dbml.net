using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a project setting clause in the syntax tree.
/// </summary>
public sealed class UnknownProjectSettingClause : ProjectSettingClause
{
    internal UnknownProjectSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken settingNameToken,
        SyntaxToken? colonToken = null,
        SyntaxToken? settingValueToken = null)
        : base(syntaxTree)
    {
        NameToken = settingNameToken;
        ColonToken = colonToken;
        ValueToken = settingValueToken;
    }

    /// <summary>
    /// Gets the syntax kind of the unknown project setting clause <see cref="SyntaxKind.UnknownProjectSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UnknownProjectSettingClause;

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
    /// Gets the children of the unknown project setting.
    /// </summary>
    /// <returns>The children of the unknown project setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NameToken;
        if (ColonToken is not null) yield return ColonToken;
        if (ValueToken is not null) yield return ValueToken;
    }
}
