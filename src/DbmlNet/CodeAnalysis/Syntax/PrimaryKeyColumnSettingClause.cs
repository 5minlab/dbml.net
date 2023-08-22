using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class PrimaryKeyColumnSettingClause : ColumnSettingClause
{
    internal PrimaryKeyColumnSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken primaryKeyword,
        SyntaxToken keyKeyword)
        : base(syntaxTree)
    {
        PrimaryKeyword = primaryKeyword;
        KeyKeyword = keyKeyword;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.PrimaryKeyColumnSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken PrimaryKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken KeyKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return PrimaryKeyword;
        yield return KeyKeyword;
    }
}
