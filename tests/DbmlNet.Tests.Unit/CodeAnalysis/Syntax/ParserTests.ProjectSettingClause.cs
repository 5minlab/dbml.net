using System.Collections.Immutable;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_DatabaseProviderProjectSettingClause_With_QuotationMarksString_Value()
    {
        SyntaxKind providerKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string providerText = $"\"{randomText}\"";
        object? providerValue = randomText;
        string settingText = $"database_type: {providerText}";
        string text = $"Project {CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax settings = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(settings);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.DatabaseProviderProjectSettingClause);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }

    [Fact]
    public void Parse_DatabaseProviderProjectSettingClause_With_SingleQuotationMarksString_Value()
    {
        SyntaxKind providerKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string providerText = $"\'{randomText}\'";
        object? providerValue = randomText;
        string settingText = $"database_type: {providerText}";
        string text = $"Project {CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax settings = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(settings);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.DatabaseProviderProjectSettingClause);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }

    [Fact]
    public void Parse_NoteProjectSettingClause_With_QuotationMarksString_Value()
    {
        SyntaxKind settingKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string noteValueText = $"\"{randomText}\"";
        object? noteValue = randomText;
        string settingText = $"note: {noteValueText}";
        string text = $"Project {CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.NoteProjectSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, noteValueText, noteValue);
    }

    [Fact]
    public void Parse_NoteProjectSettingClause_With_SingleQuotationMarksString_Value()
    {
        SyntaxKind settingKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string noteValueText = $"\'{randomText}\'";
        object? noteValue = randomText;
        string settingText = $"note: {noteValueText}";
        string text = $"Project {CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.NoteProjectSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, noteValueText, noteValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_Warning_Diagnostic()
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
    public void Parse_UnknownProjectSettingClause_With_Simple_Setting()
    {
        SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string settingNameText = randomText;
        object? settingValue = null;
        string text = $"Project {CreateRandomString()} " + "{" + settingNameText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingKind, settingNameText, settingValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_Composed_Setting_Identifier_Value()
    {
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string randomSettingValue = CreateRandomString();
        string settingValueText = randomSettingValue;
        object? settingValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_Composed_Setting_QuotationMarksString_Value()
    {
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSettingValue = CreateRandomMultiWordString();
        string settingValueText = $"\"{randomSettingValue}\"";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_Composed_Setting_SingleQuotationMarksString_Value()
    {
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSettingValue = CreateRandomMultiWordString();
        string settingValueText = $"\'{randomSettingValue}\'";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }
}
