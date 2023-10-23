namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a relationship declaration statement in the syntax tree.
/// </summary>
public abstract class RelationshipDeclarationSyntax : MemberSyntax
{
    private protected RelationshipDeclarationSyntax(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }
}
