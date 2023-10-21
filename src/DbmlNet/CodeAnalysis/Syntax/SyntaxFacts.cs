using System;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a syntax tree.
/// </summary>
public static class SyntaxFacts
{
    /// <summary>
    /// Gets the keyword kind for the given text.
    /// </summary>
    /// <param name="text">The text to get the keyword kind for.</param>
    /// <returns>The keyword kind for the given text.</returns>
    public static SyntaxKind GetKeywordKind(string text)
    {
        return text switch
        {
            // Keywords
            "database_type" => SyntaxKind.DatabaseTypeKeyword,
            "default" => SyntaxKind.DefaultKeyword,
            "false" => SyntaxKind.FalseKeyword,
            "increment" => SyntaxKind.IncrementKeyword,
            "indexes" => SyntaxKind.IndexesKeyword,
            "Indexes" => SyntaxKind.IndexesKeyword,
            "name" => SyntaxKind.NameKeyword,
            "note" => SyntaxKind.NoteKeyword,
            "Note" => SyntaxKind.NoteKeyword,
            "not" => SyntaxKind.NotKeyword,
            "null" => SyntaxKind.NullKeyword,
            "pk" => SyntaxKind.PkKeyword,
            "primary" => SyntaxKind.PrimaryKeyword,
            "key" => SyntaxKind.KeyKeyword,
            "Project" => SyntaxKind.ProjectKeyword,
            "ref" => SyntaxKind.RefKeyword,
            "Table" => SyntaxKind.TableKeyword,
            "true" => SyntaxKind.TrueKeyword,
            "type" => SyntaxKind.TypeKeyword,
            "unique" => SyntaxKind.UniqueKeyword,

            _ => SyntaxKind.IdentifierToken,
        };
    }

    /// <summary>
    /// A flag indicating whether the given kind is a <see cref="MemberSyntax"/>.
    /// </summary>
    /// <param name="kind">The syntax kind to check.</param>
    /// <returns>
    /// <see langword="true"/> if the given kind is a <see cref="MemberSyntax"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsSyntaxMember(this SyntaxKind kind)
    {
        return kind.ToString().EndsWith("Member", StringComparison.InvariantCulture)
            || kind.ToString().EndsWith("Clause", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// A flag indicating whether the given kind is a <see cref="StatementSyntax"/>.
    /// </summary>
    /// <param name="kind">The syntax kind to check.</param>
    /// <returns><see langword="true"/> if the given kind is a <see cref="StatementSyntax"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsSyntaxStatement(this SyntaxKind kind)
    {
        return kind.ToString()
            .EndsWith("Statement", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// A flag indicating whether the given kind is a <see cref="ExpressionSyntax"/>.
    /// </summary>
    /// <param name="kind">
    /// The syntax kind to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the given kind is a <see cref="ExpressionSyntax"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsSyntaxExpression(this SyntaxKind kind)
    {
        return kind.ToString()
            .EndsWith("Expression", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// A flag indicating whether the given kind is a keyword.
    /// </summary>
    /// <param name="kind">The syntax kind to check.</param>
    /// <returns><see langword="true"/> if the given kind is a keyword; otherwise, <see langword="false"/>.</returns>
    public static bool IsKeyword(this SyntaxKind kind)
    {
        return kind.ToString().EndsWith("Keyword", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// A flag indicating whether the given kind is a trivia.
    /// </summary>
    /// <param name="kind">The syntax kind to check.</param>
    /// <returns><see langword="true"/> if the given kind is a trivia; otherwise, <see langword="false"/>.</returns>
    public static bool IsTrivia(this SyntaxKind kind)
    {
        switch (kind)
        {
            case SyntaxKind.LineBreakTrivia:
            case SyntaxKind.WhitespaceTrivia:
            case SyntaxKind.SingleLineCommentTrivia:
            case SyntaxKind.MultiLineCommentTrivia:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// A flag indicating whether the given kind is a token.
    /// </summary>
    /// <param name="kind">The syntax kind to check.</param>
    /// <returns><see langword="true"/> if the given kind is a token; otherwise, <see langword="false"/>.</returns>
    public static bool IsToken(this SyntaxKind kind)
    {
        return kind.IsKeyword()
            || kind.ToString().EndsWith("Token", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// A flag indicating whether the given kind is a string token.
    /// </summary>
    /// <param name="kind">The syntax kind to check.</param>
    /// <returns><see langword="true"/> if the given kind is a string token; otherwise, <see langword="false"/>.</returns>
    public static bool IsStringToken(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.QuotationMarksStringToken => true,
            SyntaxKind.SingleQuotationMarksStringToken => true,
            SyntaxKind.MultiLineStringToken => true,
            _ => false
        };
    }

    /// <summary>
    /// A flag indicating whether the given kind is a known value.
    /// </summary>
    /// <param name="kind">The syntax kind to check.</param>
    /// <returns><see langword="true"/> if the given kind is a known value; otherwise, <see langword="false"/>.</returns>
    public static object? GetKnownValue(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.FalseKeyword => false,
            SyntaxKind.TrueKeyword => true,
            _ => null
        };
    }

    /// <summary>
    /// Gets the known text for the given syntax kind.
    /// </summary>
    /// <param name="kind">The syntax kind to check.</param>
    /// <returns>The text for the given syntax kind.</returns>
    public static string? GetKnownText(this SyntaxKind kind)
    {
        return kind switch
        {
            // Keywords
            SyntaxKind.DatabaseTypeKeyword => "database_type",
            SyntaxKind.DefaultKeyword => "default",
            SyntaxKind.FalseKeyword => "false",
            SyntaxKind.IncrementKeyword => "increment",
            SyntaxKind.IndexesKeyword => "indexes",
            SyntaxKind.NameKeyword => "name",
            SyntaxKind.NoteKeyword => "note",
            SyntaxKind.NotKeyword => "not",
            SyntaxKind.NullKeyword => "null",
            SyntaxKind.PkKeyword => "pk",
            SyntaxKind.PrimaryKeyword => "primary",
            SyntaxKind.KeyKeyword => "key",
            SyntaxKind.ProjectKeyword => "Project",
            SyntaxKind.RefKeyword => "ref",
            SyntaxKind.TableKeyword => "Table",
            SyntaxKind.TrueKeyword => "true",
            SyntaxKind.TypeKeyword => "type",
            SyntaxKind.UniqueKeyword => "unique",

            // Tokens
            SyntaxKind.DotToken => ".",
            SyntaxKind.MinusToken => "-",
            SyntaxKind.PlusToken => "+",
            SyntaxKind.SlashToken => "/",
            SyntaxKind.StarToken => "*",
            SyntaxKind.CommaToken => ",",
            SyntaxKind.ColonToken => ":",
            SyntaxKind.LessToken => "<",
            SyntaxKind.LessGraterToken => "<>",
            SyntaxKind.GraterToken => ">",
            SyntaxKind.OpenParenthesisToken => "(",
            SyntaxKind.CloseParenthesisToken => ")",
            SyntaxKind.OpenBraceToken => "{",
            SyntaxKind.CloseBraceToken => "}",
            SyntaxKind.OpenBracketToken => "[",
            SyntaxKind.CloseBracketToken => "]",
            SyntaxKind.BacktickToken => "`",

            _ => null
        };
    }
}
