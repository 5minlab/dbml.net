using System.Collections.Generic;
using System.Diagnostics;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a composite index declaration statement.
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
    /// Gets the syntax kind of the composite index declaration statement <see cref="SyntaxKind.CompositeIndexDeclarationStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.CompositeIndexDeclarationStatement;

    /// <summary>
    /// Gets the open parenthesis token.
    /// </summary>
    public SyntaxToken OpenParenthesis { get; }

    /// <summary>
    /// Gets the identifiers.
    /// </summary>
    public SeparatedSyntaxList<ExpressionSyntax> Identifiers { get; }

    /// <summary>
    /// Gets the close parenthesis token.
    /// </summary>
    public SyntaxToken CloseParenthesis { get; }

    /// <summary>
    /// Gets the open bracket token.
    /// </summary>
    public SyntaxToken? OpenBracket { get; }

    /// <summary>
    /// Gets the settings.
    /// </summary>
    public SeparatedSyntaxList<ExpressionSyntax>? Settings { get; }

    /// <summary>
    /// Gets the close bracket token.
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
