using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class UniqueColumnSettingClause : ColumnSettingClause
{
    internal UniqueColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken uniqueKeyword)
        : base(syntaxTree)
    {
        UniqueKeyword = uniqueKeyword;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UniqueColumnSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken UniqueKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return UniqueKeyword;
    }
}
