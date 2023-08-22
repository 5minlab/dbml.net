namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public abstract class ExpressionSyntax : SyntaxNode
{
    private protected ExpressionSyntax(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
