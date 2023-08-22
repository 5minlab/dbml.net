using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class DatabaseProviderDeclarationSyntax : StatementSyntax
{
    internal DatabaseProviderDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken databaseTypeKeyword,
        SyntaxToken colonToken,
        SyntaxToken identifierToken)
        : base(syntaxTree)
    {
        DatabaseTypeKeyword = databaseTypeKeyword;
        ColonToken = colonToken;
        IdentifierToken = identifierToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.DatabaseProviderDeclarationStatement;

    /// <summary>
    /// </summary>
    public SyntaxToken DatabaseTypeKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return DatabaseTypeKeyword;
        yield return ColonToken;
        yield return IdentifierToken;
    }
}
