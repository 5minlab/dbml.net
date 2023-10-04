using System.Collections.Generic;
using System.Diagnostics;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class CompositeIndexDeclarationSyntax : StatementSyntax
{
    internal CompositeIndexDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken openParenthesis,
        SeparatedSyntaxList<ExpressionSyntax> identifiers,
        SyntaxToken closeParenthesis,
        SyntaxToken? openBracket = null,
        SeparatedSyntaxList<ExpressionSyntax>? settings = null,
        SyntaxToken? closeBracket = null)
        : base(syntaxTree)
    {
        OpenParenthesis = openParenthesis;
        Identifiers = identifiers;
        CloseParenthesis = closeParenthesis;
        OpenBracket = openBracket;
        Settings = settings;
        CloseBracket = closeBracket;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.CompositeIndexDeclarationStatement;

    /// <summary>
    /// </summary>
    public SyntaxToken OpenParenthesis { get; }

    /// <summary>
    /// </summary>
    public SeparatedSyntaxList<ExpressionSyntax> Identifiers { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken CloseParenthesis { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken? OpenBracket { get; }

    /// <summary>
    /// </summary>
    public SeparatedSyntaxList<ExpressionSyntax>? Settings { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken? CloseBracket { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenParenthesis;

        foreach (SyntaxNode setting in Identifiers.GetWithSeparators())
            yield return setting;

        yield return CloseParenthesis;

        if (OpenBracket is not null)
        {
            yield return OpenBracket;

            Debug.Assert(Settings is not null);
            foreach (ExpressionSyntax setting in Settings)
                yield return setting;

            Debug.Assert(CloseBracket is not null);
            yield return CloseBracket;
        }
    }
}
