namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public abstract class MemberSyntax : SyntaxNode
{
    private protected MemberSyntax(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
