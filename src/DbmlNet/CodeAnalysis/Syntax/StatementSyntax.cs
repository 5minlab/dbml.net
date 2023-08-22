namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public abstract class StatementSyntax : SyntaxNode
{
    private protected StatementSyntax(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
