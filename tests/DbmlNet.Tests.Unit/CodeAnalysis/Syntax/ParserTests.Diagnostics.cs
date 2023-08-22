using System.Collections.Generic;
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
        Assert.Equal($"Bad character input: '{text}'.", diagnostic.Message);
        Assert.True(diagnostic.IsError, "Diagnostic show be error.");
        Assert.False(diagnostic.IsWarning, "Diagnostic show not be warning.");
    }
}
