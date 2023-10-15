using System.Collections.Immutable;

using DbmlNet.CodeAnalysis;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Theory]
    [InlineData("!")]
    [InlineData("?")]
    [InlineData("|")]
    public void Parse_Skip_BadToken(string text)
    {
        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        string expectedDiagnosticMessage = $"Bad character input: '{text}'.";
        Assert.Equal(expectedDiagnosticMessage, diagnostic.Message);
        Assert.Equal(expectedDiagnosticMessage, $"{diagnostic}");
        Assert.True(diagnostic.IsError, "Diagnostic should be an error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic should not be an warning.");
        Assert.Equal(0, diagnostic.Location.StartLine);
        Assert.Equal(0, diagnostic.Location.EndLine);
        Assert.Equal(0, diagnostic.Location.StartCharacter);
        Assert.Equal(1, diagnostic.Location.EndCharacter);
        Assert.Equal(0, diagnostic.Location.Span.Start);
        Assert.Equal(1, diagnostic.Location.Span.End);
    }
}
