using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a project setting clause in the syntax tree.
/// </summary>
public sealed class TypeIndexSettingClause : IndexSettingClause
{
    internal TypeIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken typeKeyword,
        SyntaxToken colonToken,
        SyntaxToken valueToken)
        : base(syntaxTree)
    {
        TypeKeyword = typeKeyword;
        ColonToken = colonToken;
        ValueToken = valueToken;
        SettingName = $"{typeKeyword.Value ?? typeKeyword.Text}";
    }

    /// <summary>
    /// Gets the syntax kind of the type index setting clause <see cref="SyntaxKind.TypeIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.TypeIndexSettingClause;

    /// <inherits/>
    public override string SettingName { get; }

    /// <summary>
    /// Gets the type keyword.
    /// </summary>
    public SyntaxToken TypeKeyword { get; }

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
        yield return TypeKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
