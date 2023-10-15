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
        IndexSettingListSyntax? settings = null)
        : base(syntaxTree)
    {
        OpenParenthesis = openParenthesis;
        Identifiers = identifiers;
        CloseParenthesis = closeParenthesis;
        Settings = settings;
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
    /// Gets the settings.
    /// </summary>
    public IndexSettingListSyntax? Settings { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenParenthesis;

        foreach (SyntaxNode setting in Identifiers.GetWithSeparators())
            yield return setting;

        yield return CloseParenthesis;

        if (Settings is not null) yield return Settings;
    }
}
