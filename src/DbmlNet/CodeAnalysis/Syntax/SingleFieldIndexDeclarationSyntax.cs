using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a single field index declaration statement in the syntax tree.
/// </summary>
public sealed class SingleFieldIndexDeclarationSyntax : IndexDeclarationStatementSyntax
{
    internal SingleFieldIndexDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken identifierToken,
        IndexSettingListSyntax? settings)
        : base(syntaxTree)
    {
        IdentifierToken = identifierToken;
        Settings = settings;
        IdentifierName = identifierToken.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the single field index declaration <see cref="SyntaxKind.SingleFieldIndexDeclarationStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.SingleFieldIndexDeclarationStatement;

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public override string IdentifierName { get; }

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// Gets the index setting list.
    /// </summary>
    public IndexSettingListSyntax? Settings { get; }

    /// <summary>
    /// Gets the children of the single field index declaration.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
        if (Settings is not null) yield return Settings;
    }
}
