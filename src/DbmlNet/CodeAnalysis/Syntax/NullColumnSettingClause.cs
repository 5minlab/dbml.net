using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class NullColumnSettingClause : ColumnSettingClause
{
    internal NullColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken nullKeyword)
        : base(syntaxTree)
    {
        NullKeyword = nullKeyword;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NullColumnSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken NullKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NullKeyword;
    }
}
