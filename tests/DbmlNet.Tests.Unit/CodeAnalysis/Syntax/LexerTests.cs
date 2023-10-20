using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.CodeAnalysis.Text;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public class LexerTests
{
    [Fact]
    public void Lexer_Throws_ArgumentNullException_For_null_syntax()
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(string.Empty, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.EndOfFileToken, token.Kind);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Empty(diagnostics);
    }

    [Fact]
    public void Lexer_Lex_EndOfFile()
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(string.Empty, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.EndOfFileToken, token.Kind);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Empty(diagnostics);
    }

    [Theory]
    [InlineData("!")]
    [InlineData("?")]
    [InlineData("|")]
    public void Lexer_Lex_BadToken(string text)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.BadToken, token.Kind);
        Assert.False(token.IsMissing);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Equal($"Bad character input: '{text}'.", diagnostic.Message);
        Assert.True(diagnostic.IsError, "Diagnostic should be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic should not be warning.");
    }

    [Theory]
    [InlineData("\t")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void Lexer_Lex_Trivia_Whitespace(string text)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        SyntaxToken token = Assert.Single(tokens);
        SyntaxTrivia trivia = Assert.Single(token.LeadingTrivia);
        Assert.Equal(SyntaxKind.WhitespaceTrivia, trivia.Kind);
        Assert.Empty(diagnostics);
    }

    [Theory]
    [InlineData("\r")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Lexer_Lex_Trivia_LineBreak(string text)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        SyntaxTrivia trivia = Assert.Single(token.LeadingTrivia);
        Assert.Equal(text, trivia.Text);
        Assert.Equal(SyntaxKind.LineBreakTrivia, trivia.Kind);
    }

    [Fact]
    public void Lexer_Lex_Trivia_SingleLineComment()
    {
        string text = $"// {DataGenerator.CreateRandomMultiWordString()}";

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics, includeEndOfFile: true);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        SyntaxTrivia trivia = Assert.Single(token.LeadingTrivia);
        Assert.Equal(text, trivia.Text);
        Assert.Equal(SyntaxKind.SingleLineCommentTrivia, trivia.Kind);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    [InlineData("1234567890", 1234567890)]
    public void Lexer_Lex_Number(string text, decimal value)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.NumberToken, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.IsType<decimal>(token.Value);
        Assert.Equal(value, token.Value);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Empty(diagnostics);
    }

    [Theory]
    [InlineData("0.2232", 0.2232)]
    [InlineData("12345.67890", 12345.67890)]
    public void Lexer_Lex_Number_WithDecimals(string text, decimal value)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.NumberToken, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.IsType<decimal>(token.Value);
        Assert.Equal(value, token.Value);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Empty(diagnostics);
    }

    [Theory]
    [InlineData("0__.0_0_", 0.00)]
    [InlineData("2_232.2_232", 2232.2232)]
    [InlineData("2__23__2.2__23__2____", 2232.2232)]
    [InlineData("1__0__0_0___.__21_22_1____", 1000.21221)]
    public void Lexer_Lex_Number_WithSeparators(string text, decimal value)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.NumberToken, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.IsType<decimal>(token.Value);
        Assert.Equal(value, token.Value);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Empty(diagnostics);
    }

    [Fact]
    public void Lexer_Lex_Number_WhenNumberTooLarge()
    {
        const string text = "99999999999999999999999999999.9";

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(text, token.Text);
        Assert.Equal(SyntaxKind.NumberToken, token.Kind);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Null(token.Value);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Equal("The number '99999999999999999999999999999.9' is too large.", diagnostic.Message);
        Assert.True(diagnostic.IsError, "Diagnostic show be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic show not be warning.");
    }

    [Fact]
    public void Lexer_Lex_QuotationMarksStringToken()
    {
        const string textValue = """
        error: ...the "message" is "my message".
        """;
        const string text = """
        "error: ...the ""message"" is ""my message""."
        """;

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.QuotationMarksStringToken, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.IsType<string>(token.Value);
        Assert.Equal(textValue, token.Value);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Empty(diagnostics);
    }

    [Fact]
    public void Lexer_Lex_SingleQuotationMarksStringToken()
    {
        const string textValue = """
        error: ...the 'message' is 'my message'.
        """;
        const string text = """
        'error: ...the ''message'' is ''my message''.'
        """;

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.SingleQuotationMarksStringToken, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.IsType<string>(token.Value);
        Assert.Equal(textValue, token.Value);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Empty(diagnostics);
    }

    [Fact]
    public void Lexer_Lex_Unterminated_QuotationMarksStringToken()
    {
        const string text = "\"text";

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.QuotationMarksStringToken, token.Kind);
        Assert.Equal(text, token.Text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Equal(new TextSpan(0, 1), diagnostic.Location.Span);
        Assert.Equal("Unterminated string literal.", diagnostic.Message);
    }

    [Fact]
    public void Lexer_Lex_Unterminated_SingleQuotationMarksStringToken()
    {
        const string text = "\'text";

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.SingleQuotationMarksStringToken, token.Kind);
        Assert.Equal(text, token.Text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Equal(new TextSpan(0, 1), diagnostic.Location.Span);
        Assert.Equal("Unterminated string literal.", diagnostic.Message);
    }

    [Fact]
    public void Lexer_Covers_AllTokens()
    {
        IEnumerable<SyntaxKind> tokenKinds =
            Enum.GetValues(typeof(SyntaxKind))
                .Cast<SyntaxKind>()
                .Where(k => k.IsToken());

        IEnumerable<SyntaxKind> testedTokenKinds =
            GetTokens().Select(t => t.Kind);

        SortedSet<SyntaxKind> untestedTokenKinds =
            new SortedSet<SyntaxKind>(tokenKinds);

        untestedTokenKinds.Remove(SyntaxKind.BadToken);
        untestedTokenKinds.Remove(SyntaxKind.EndOfFileToken);
        untestedTokenKinds.ExceptWith(testedTokenKinds);

        Assert.Empty(untestedTokenKinds);
    }

    [Theory]
    [MemberData(nameof(GetTokensData), DisableDiscoveryEnumeration = true)]
    public void Lexer_Lex_Token(SyntaxKind kind, string text)
    {
        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(kind, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.False(token.IsMissing);
        Assert.Empty(diagnostics);
    }

    public static IEnumerable<object[]> GetTokensData()
    {
        foreach ((SyntaxKind kind, string text) in GetTokens())
            yield return new object[] { kind, text };
    }

    private static IEnumerable<(SyntaxKind Kind, string Text)> GetTokens()
    {
        IEnumerable<(SyntaxKind Kind, string Text)> fixedTokens =
            Enum.GetValues<SyntaxKind>()
                .Select(kind => (Kind: kind, Text: SyntaxFacts.GetKnownText(kind) ?? null!))
                .Where(t => !string.IsNullOrEmpty(t.Text));

        (SyntaxKind, string)[] dynamicTokens = new[]
        {
            (SyntaxKind.NumberToken, "1"),
            (SyntaxKind.NumberToken, "123"),
            (SyntaxKind.NumberToken, "1_123"),
            (SyntaxKind.NumberToken, "1.1"),
            (SyntaxKind.NumberToken, "123.123"),
            (SyntaxKind.NumberToken, "1_123.1_123"),
            (SyntaxKind.NumberToken, $"{DataGenerator.GetRandomNumber(min: 0)}"),
            (SyntaxKind.NumberToken, $"{DataGenerator.GetRandomDecimal(min: 0)}"),

            (SyntaxKind.QuotationMarksStringToken, "\"Test\""),
            (SyntaxKind.QuotationMarksStringToken, $"\"...message: \"\"message here\"\"\""),
            (SyntaxKind.QuotationMarksStringToken, $"\"{DataGenerator.CreateRandomMultiWordString()}\""),
            (SyntaxKind.SingleQuotationMarksStringToken, "\'Test\'"),
            (SyntaxKind.SingleQuotationMarksStringToken, $"\'...message: \'\'message here\'\'\'"),
            (SyntaxKind.SingleQuotationMarksStringToken, $"\'{DataGenerator.CreateRandomMultiWordString()}\'"),

            (SyntaxKind.IdentifierToken, "a"),
            (SyntaxKind.IdentifierToken, "abc"),
            (SyntaxKind.IdentifierToken, "_a_b_c"),
        };

        return fixedTokens.Concat(dynamicTokens);
    }
}
