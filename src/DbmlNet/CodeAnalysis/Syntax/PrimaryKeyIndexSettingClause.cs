using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class PrimaryKeyIndexSettingClause : IndexSettingClause
{
    internal PrimaryKeyIndexSettingClause(
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
    public override SyntaxKind Kind => SyntaxKind.PrimaryKeyIndexSettingClause;

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
