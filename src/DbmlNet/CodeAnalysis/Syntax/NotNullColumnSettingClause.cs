using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class NotNullColumnSettingClause : ColumnSettingClause
{
    internal NotNullColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken notKeyword,
        SyntaxToken nullKeyword)
        : base(syntaxTree)
    {
        NotKeyword = notKeyword;
        NullKeyword = nullKeyword;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NotNullColumnSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken NotKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken NullKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NotKeyword;
        yield return NullKeyword;
    }
}
