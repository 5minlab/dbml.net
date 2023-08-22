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
        SyntaxToken? openBracketToken,
        SeparatedSyntaxList<ExpressionSyntax>? settings,
        SyntaxToken? closeBracketToken)
        : base(syntaxTree)
    {
        IdentifierToken = identifierToken;
        OpenBracketToken = openBracketToken;
        Settings = settings;
        CloseBracketToken = closeBracketToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.SingleFieldIndexDeclarationStatement;

    /// <summary>
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken? OpenBracketToken { get; }

    /// <summary>
    /// </summary>
    public SeparatedSyntaxList<ExpressionSyntax>? Settings { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken? CloseBracketToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return IdentifierToken;
        if (OpenBracketToken is not null)
        {
            yield return OpenBracketToken;

            Debug.Assert(Settings is not null);
            foreach (ExpressionSyntax setting in Settings)
                yield return setting;

            Debug.Assert(CloseBracketToken is not null);
            yield return CloseBracketToken;
        }
    }
}
