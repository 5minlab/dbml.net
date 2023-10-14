namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column type clause in the syntax tree.
/// </summary>
public abstract class ColumnTypeClause : MemberSyntax
{
    private protected ColumnTypeClause(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
