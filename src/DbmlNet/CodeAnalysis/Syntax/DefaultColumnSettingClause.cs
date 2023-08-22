using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class DefaultColumnSettingClause : ColumnSettingClause
{
    internal DefaultColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken defaultKeyword,
        SyntaxToken colonToken,
        SyntaxToken valueToken)
        : base(syntaxTree)
    {
        DefaultKeyword = defaultKeyword;
        ColonToken = colonToken;
        ValueToken = valueToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.DefaultColumnSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken DefaultKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ValueToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return DefaultKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
