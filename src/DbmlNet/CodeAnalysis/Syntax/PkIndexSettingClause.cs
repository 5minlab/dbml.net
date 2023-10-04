using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class PkIndexSettingClause : IndexSettingClause
{
    internal PkIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken pkKeyword)
        : base(syntaxTree)
    {
        PkKeyword = pkKeyword;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.PkIndexSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken PkKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return PkKeyword;
    }
}
