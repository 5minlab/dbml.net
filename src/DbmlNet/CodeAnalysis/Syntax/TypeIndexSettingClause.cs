using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.TypeIndexSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken TypeKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
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
