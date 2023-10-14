namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a project setting clause in the syntax tree.
/// </summary>
public abstract class ProjectSettingClause : SyntaxNode
{
    private protected ProjectSettingClause(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
