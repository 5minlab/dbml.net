using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UnknownProjectSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken NameToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken? ColonToken { get; }

    /// <summary>
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
