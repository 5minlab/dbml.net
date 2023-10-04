using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class UniqueIndexSettingClause : IndexSettingClause
{
    internal UniqueIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken uniqueKeyword)
        : base(syntaxTree)
    {
        UniqueKeyword = uniqueKeyword;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.UniqueIndexSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken UniqueKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return UniqueKeyword;
    }
}
