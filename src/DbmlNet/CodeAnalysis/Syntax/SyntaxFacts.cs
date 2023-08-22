using System;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public static class SyntaxFacts
{
    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
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
            "name" => SyntaxKind.NameKeyword,
            "note" => SyntaxKind.NoteKeyword,
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
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static bool IsSyntaxMember(this SyntaxKind kind)
    {
        return kind.ToString().EndsWith("Member", StringComparison.InvariantCulture)
            || kind.ToString().EndsWith("Clause", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static bool IsSyntaxStatement(this SyntaxKind kind)
    {
        return kind.ToString()
            .EndsWith("Statement", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static bool IsSyntaxExpression(this SyntaxKind kind)
    {
        return kind.ToString()
            .EndsWith("Expression", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static bool IsKeyword(this SyntaxKind kind)
    {
        return kind.ToString().EndsWith("Keyword", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static bool IsToken(this SyntaxKind kind)
    {
        return kind.IsKeyword()
            || kind.ToString().EndsWith("Token", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static bool IsStringToken(this SyntaxKind kind)
    {
        return kind == SyntaxKind.QuotationMarksStringToken
            || kind == SyntaxKind.SingleQuotationMarksStringToken;
    }

    /// <summary>
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
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
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
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

            _ => null
        };
    }
}
