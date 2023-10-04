using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class NameIndexSettingClause : IndexSettingClause
{
    internal NameIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken nameKeyword,
        SyntaxToken colonToken,
        SyntaxToken valueToken)
        : base(syntaxTree)
    {
        NameKeyword = nameKeyword;
        ColonToken = colonToken;
        ValueToken = valueToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NameIndexSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken NameKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ValueToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NameKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
