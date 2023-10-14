using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a index setting clause in the syntax tree.
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
    /// Gets the syntax kind of the index setting clause <see cref="SyntaxKind.PkIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.PkIndexSettingClause;

    /// <summary>
    /// Gets the pk keyword.
    /// </summary>
    public SyntaxToken PkKeyword { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return PkKeyword;
    }
}
