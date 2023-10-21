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
        ExpressionSyntax expressionValue)
        : base(syntaxTree)
    {
        DefaultKeyword = defaultKeyword;
        ColonToken = colonToken;
        ExpressionValue = expressionValue;
        SettingName = defaultKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the default column setting clause <see cref="SyntaxKind.DefaultColumnSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.DefaultColumnSettingClause;

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public override string SettingName { get; }

    /// <summary>
    /// Gets the default keyword.
    /// </summary>
    public SyntaxToken DefaultKeyword { get; }

    /// <summary>
    /// Gets the colon token.
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// Gets the expression value.
    /// </summary>
    public ExpressionSyntax ExpressionValue { get; }

    /// <summary>
    /// Gets the children of the default column setting.
    /// </summary>
    /// <returns>The children of the default column setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return DefaultKeyword;
        yield return ColonToken;
        yield return ExpressionValue;
    }
}
