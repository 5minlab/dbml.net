using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class ColumnTypeIdentifierClause : ColumnTypeClause
{
    internal ColumnTypeIdentifierClause(SyntaxTree syntaxTree, SyntaxToken columnTypeToken)
        : base(syntaxTree)
    {
        ColumnTypeIdentifier = columnTypeToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnTypeIdentifierClause;

    /// <summary>
    /// </summary>
    public SyntaxToken ColumnTypeIdentifier { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ColumnTypeIdentifier;
    }
}
