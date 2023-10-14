namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a member in the syntax tree.
/// </summary>
public abstract class MemberSyntax : SyntaxNode
{
    private protected MemberSyntax(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
