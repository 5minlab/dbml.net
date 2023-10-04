using System.Collections.Generic;
using System.Diagnostics;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.SingleFieldIndexDeclarationStatement;

    /// <summary>
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// </summary>
    public IndexSettingListSyntax? Settings { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
        if (Settings is not null) yield return Settings;
    }
}
