namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a index setting clause in the syntax tree.
/// </summary>
public abstract class IndexSettingClause : SyntaxNode
{
    private protected IndexSettingClause(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
