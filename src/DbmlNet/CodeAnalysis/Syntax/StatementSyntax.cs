namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a statement in the syntax tree.
/// </summary>
public abstract class StatementSyntax : SyntaxNode
{
    private protected StatementSyntax(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
