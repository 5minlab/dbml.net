namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a table setting clause in the syntax tree.
/// </summary>
public abstract class TableSettingClause : SyntaxNode
{
    private protected TableSettingClause(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
