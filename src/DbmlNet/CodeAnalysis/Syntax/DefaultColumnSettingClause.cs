using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a default column setting clause in the syntax tree.
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
    /// Gets the syntax kind of the default column setting clause <see cref="SyntaxKind.DefaultColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.DefaultColumnSettingClause;

    /// <summary>
    /// Gets the default keyword.
    /// </summary>
    public SyntaxToken DefaultKeyword { get; }

    /// <summary>
    /// Gets the colon token.
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// Gets the value token.
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
