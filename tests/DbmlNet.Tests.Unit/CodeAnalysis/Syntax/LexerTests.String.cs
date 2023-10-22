using System.Collections.Immutable;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.CodeAnalysis.Text;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class LexerTests
{
    [Fact]
    public void Lexer_Lex_String_QuotationMarksString()
    {
        const string textValue = """
        error: ...the "message" is "my message".
        """;
        const string text = """
        "error: ...the ""message"" is ""my message""."
        """;

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.QuotationMarksStringToken, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.IsType<string>(token.Value);
        Assert.Equal(textValue, token.Value);
        Assert.False(token.IsMissing, "Token should not be missing.");
    }

    [Fact]
    public void Lexer_Lex_String_SingleQuotationMarksString()
    {
        const string textValue = """
        error: ...the 'message' is 'my message'.
        """;
        const string text = """
        'error: ...the ''message'' is ''my message''.'
        """;

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.SingleQuotationMarksStringToken, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.IsType<string>(token.Value);
        Assert.Equal(textValue, token.Value);
        Assert.False(token.IsMissing, "Token should not be missing.");
    }

    [Fact]
    public void Lexer_Lex_String_MultiLineString()
    {
        const string textValue = """
        This is a block of string
        This string can spans over multiple lines.
        error: \ ...the '''message''' is '''my message'''.
        """;
        const string text = """
        '''
            This is a block of string
            This string can spans over multiple lines.
            error: \\ ...the \'''message\''' is \'''my message\'''.
        '''
        """;

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Assert.Empty(diagnostics);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.MultiLineStringToken, token.Kind);
        Assert.Equal(text, token.Text);
        Assert.IsType<string>(token.Value);
        Assert.Equal(textValue, token.Value);
        Assert.False(token.IsMissing, "Token should not be missing.");
    }

    [Fact]
    public void Lexer_Lex_String_Unterminated_QuotationMarksString()
    {
        const string text = "\"text";

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.True(diagnostic.IsError, "Diagnostic show be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic show not be warning.");
        Assert.Equal(new TextSpan(0, 1), diagnostic.Location.Span);
        Assert.Equal("Unterminated string literal.", diagnostic.Message);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.QuotationMarksStringToken, token.Kind);
        Assert.Equal(text, token.Text);
    }

    [Fact]
    public void Lexer_Lex_String_Unterminated_SingleQuotationMarksString()
    {
        const string text = "\'text";

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.True(diagnostic.IsError, "Diagnostic show be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic show not be warning.");
        Assert.Equal(new TextSpan(0, 1), diagnostic.Location.Span);
        Assert.Equal("Unterminated string literal.", diagnostic.Message);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.SingleQuotationMarksStringToken, token.Kind);
        Assert.Equal(text, token.Text);
    }

    [Fact]
    public void Lexer_Lex_String_Unterminated_MultiLineString()
    {
        const string text = """'''text""";

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.True(diagnostic.IsError, "Diagnostic show be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic show not be warning.");
        Assert.Equal(new TextSpan(0, 3), diagnostic.Location.Span);
        Assert.Equal("Unterminated multi-line string literal.", diagnostic.Message);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.MultiLineStringToken, token.Kind);
        Assert.Equal(text, token.Text);
    }

    [Theory]
    [InlineData("""\\\""", 5, 6)]
    [InlineData("""\n""", 3, 4)]
    public void Lexer_Lex_String_Unrecognized_Escape_Sequence_In_MultiLineString(
        string unrecognizedText, int start, int end)
    {
        Assert.True(start >= 3, "Invalid test input: start >= 3, from length(```).");
        Assert.True(end >= 3, "Invalid test input: start >= 3, from length(```).");

        string text = $"""
        '''{unrecognizedText} unrecognized escape sequence.'''
        """;

        ImmutableArray<SyntaxToken> tokens =
            SyntaxTree.ParseTokens(text, out ImmutableArray<Diagnostic> diagnostics);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.True(diagnostic.IsError, "Diagnostic show be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic show not be warning.");
        Assert.Equal(TextSpan.FromBounds(start, end), diagnostic.Location.Span);
        Assert.Equal("Unrecognized escape sequence.", diagnostic.Message);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(SyntaxKind.MultiLineStringToken, token.Kind);
        Assert.Equal(text, token.Text);
    }
}
