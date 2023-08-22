using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class IncrementColumnSettingClause : ColumnSettingClause
{
    internal IncrementColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken incrementKeyword)
        : base(syntaxTree)
    {
        IncrementKeyword = incrementKeyword;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.IncrementColumnSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken IncrementKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IncrementKeyword;
    }
}
