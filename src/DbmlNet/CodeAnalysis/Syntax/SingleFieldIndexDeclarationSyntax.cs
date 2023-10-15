using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a single field index declaration statement in the syntax tree.
/// </summary>
public sealed class SingleFieldIndexDeclarationSyntax : StatementSyntax
{
    internal SingleFieldIndexDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken identifierToken,
        IndexSettingListSyntax? settings)
        : base(syntaxTree)
    {
        IdentifierToken = identifierToken;
        Settings = settings;
    }

    /// <summary>
    /// Gets the syntax kind of the single field index declaration statement <see cref="SyntaxKind.SingleFieldIndexDeclarationStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.SingleFieldIndexDeclarationStatement;

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// Gets the index setting list.
    /// </summary>
    public IndexSettingListSyntax? Settings { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
        if (Settings is not null) yield return Settings;
    }
}
