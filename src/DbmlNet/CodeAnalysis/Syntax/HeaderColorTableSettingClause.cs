using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents an headercolor table setting clause in the syntax tree.
/// </summary>
public sealed class HeaderColorTableSettingClause : TableSettingClause
{
    internal HeaderColorTableSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken headerColorKeyword,
        SyntaxToken colonToken,
        SyntaxToken valueToken)
        : base(syntaxTree)
    {
        HeaderColorKeyword = headerColorKeyword;
        ColonToken = colonToken;
        ValueToken = valueToken;
    }

    /// <summary>
    /// Gets the syntax kind of the headercolor table setting clause <see cref="SyntaxKind.HeaderColorTableSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.HeaderColorTableSettingClause;

    /// <summary>
    /// Gets the headercolor keyword.
    /// </summary>
    public SyntaxToken HeaderColorKeyword { get; }

    /// <summary>
    /// Gets the colon token.
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// Gets the value token.
    /// </summary>
    public SyntaxToken ValueToken { get; }

    /// <summary>
    /// Gets the children of the headercolor table setting.
    /// </summary>
    /// <returns>The children of the headercolor table setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return HeaderColorKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
