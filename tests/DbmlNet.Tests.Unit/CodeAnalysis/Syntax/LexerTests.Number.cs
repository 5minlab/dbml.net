using System.Collections.Immutable;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class LexerTests
{
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

        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.Equal("The number '99999999999999999999999999999.9' is too large.", diagnostic.Message);
        Assert.True(diagnostic.IsError, "Diagnostic show be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic show not be warning.");
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(text, token.Text);
        Assert.Equal(SyntaxKind.NumberToken, token.Kind);
        Assert.False(token.IsMissing, "Token should not be missing.");
        Assert.Null(token.Value);
    }
}
