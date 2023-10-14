namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents an expression in the syntax tree.
/// </summary>
public abstract class ExpressionSyntax : SyntaxNode
{
    private protected ExpressionSyntax(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
