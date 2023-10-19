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

    [Fact]
    public void Parse_Warning_Unknown_Project_Setting()
    {
        string randomText = CreateRandomString();
        string settingNameText = randomText;
        string text = $"Project {CreateRandomString()} " + "{" + settingNameText + "}";

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        string expectedDiagnosticMessage = $"Unknown project setting '{settingNameText}'.";
        Assert.Equal(expectedDiagnosticMessage, diagnostic.Message);
        Assert.Equal(expectedDiagnosticMessage, $"{diagnostic}");
        Assert.True(diagnostic.IsWarning, "Diagnostic should be warning.");
        Assert.False(diagnostic.IsError, "Diagnostic should not be error.");
    }

    [Fact]
    public void Parse_Warning_Unknown_Column_Setting()
    {
        string randomText = CreateRandomString();
        string settingNameText = randomText;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ {settingNameText} ]";

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        string expectedDiagnosticMessage = $"Unknown column setting '{settingNameText}'.";
        Assert.Equal(expectedDiagnosticMessage, diagnostic.Message);
        Assert.Equal(expectedDiagnosticMessage, $"{diagnostic}");
        Assert.True(diagnostic.IsWarning, "Diagnostic should be warning.");
        Assert.False(diagnostic.IsError, "Diagnostic should not be error.");
    }

    [Fact]
    public void Parse_Warning_Table_Already_Declared()
    {
        string randomText = CreateRandomString();
        string firstTableName = randomText;
        string secondTableName = randomText;
        string text = $$"""
        Table {{firstTableName}} { }
        Table {{secondTableName}} { }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Table '{secondTableName}' already declared.", diagnostic.Message);
    }

    [Fact]
    public void Parse_Warning_Column_Already_Declared()
    {
        string randomText = CreateRandomString();
        string firstColumnName = randomText;
        string secondColumnName = randomText;
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{firstColumnName}} {{CreateRandomString()}}
            {{secondColumnName}} {{CreateRandomString()}}
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Column '{secondColumnName}' already declared.", diagnostic.Message);
    }

    [Theory]
    [InlineData("pk", "pk")]
    [InlineData("primarykey", "primary key")]
    [InlineData("null", "null")]
    [InlineData("notnull", "not null")]
    [InlineData("unique", "unique")]
    [InlineData("increment", "increment")]
    [InlineData("default", "default: Some_value")]
    [InlineData("default", "default: \"Some value\"")]
    [InlineData("default", "default: \'Some value\'")]
    [InlineData("note", "note: \"Some value\"")]
    [InlineData("note", "note: \'Some value\'")]
    public void Parse_Warning_Column_Setting_Already_Declared(
        string settingName, string settingText)
    {
        string text = $$"""
        {{CreateRandomString()}} {{CreateRandomString()}} [ {{settingText}}, {{settingText}} ]
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Column setting '{settingName}' already declared.", diagnostic.Message);
    }

    [Theory]
    [InlineData("pk", "pk")]
    [InlineData("primarykey", "primary key")]
    [InlineData("unique", "unique")]
    [InlineData("name", "name: Some_value")]
    [InlineData("name", "name: \"Some value\"")]
    [InlineData("name", "name: \'Some value\'")]
    [InlineData("type", "type: btree")]
    [InlineData("type", "type: \"btree\"")]
    [InlineData("type", "type: \'btree\'")]
    [InlineData("type", "type: gin")]
    [InlineData("type", "type: \"gin\"")]
    [InlineData("type", "type: \'gin\'")]
    [InlineData("type", "type: gist")]
    [InlineData("type", "type: \"gist\"")]
    [InlineData("type", "type: \'gist\'")]
    [InlineData("type", "type: hash")]
    [InlineData("type", "type: \"hash\"")]
    [InlineData("type", "type: \'hash\'")]
    public void Parse_Warning_Index_Setting_Already_Declared(
        string settingName, string settingText)
    {
        string text = $$"""
        indexes
        {
            {{CreateRandomString()}} [ {{settingText}}, {{settingText}} ]
        }
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Index setting '{settingName}' already declared.", diagnostic.Message);
    }
}
