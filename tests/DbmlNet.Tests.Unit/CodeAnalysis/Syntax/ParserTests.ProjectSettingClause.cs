using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_DatabaseProviderProjectSettingClause_With_QuotationMarksString_Value()
    {
        SyntaxKind providerKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string providerText = $"\"{randomText}\"";
        object? providerValue = randomText;
        string settingText = $"database_type: {providerText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

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
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string providerText = $"\'{randomText}\'";
        object? providerValue = randomText;
        string settingText = $"database_type: {providerText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax settings = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(settings);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.DatabaseProviderProjectSettingClause);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }

    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteProjectSettingClause_With_QuotationMarksString_Value(string noteKeywordText)
    {
        SyntaxKind settingKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string noteValueText = $"\"{randomText}\"";
        object? noteValue = randomText;
        string settingText = $"{noteKeywordText}: {noteValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.NoteProjectSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, noteKeywordText.ToLowerInvariant());
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, noteValueText, noteValue);
    }

    [Theory]
    [InlineData("Note")]
    [InlineData("note")]
    public void Parse_NoteProjectSettingClause_With_SingleQuotationMarksString_Value(string noteKeywordText)
    {
        SyntaxKind settingKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string noteValueText = $"\'{randomText}\'";
        object? noteValue = randomText;
        string settingText = $"{noteKeywordText}: {noteValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.NoteProjectSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, noteKeywordText.ToLowerInvariant());
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, noteValueText, noteValue);
    }

    [Fact]
    public void Parse_UnknownProjectSettingClause_With_Simple_Setting()
    {
        SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
        string settingNameText = randomText;
        object? settingValue = null;
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingNameText + "}";

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
        string randomSettingName = DataGenerator.CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string randomSettingValue = DataGenerator.CreateRandomString();
        string settingValueText = randomSettingValue;
        object? settingValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

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
        string randomSettingName = DataGenerator.CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSettingValue = DataGenerator.CreateRandomMultiWordString();
        string settingValueText = $"\"{randomSettingValue}\"";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

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
        string randomSettingName = DataGenerator.CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSettingValue = DataGenerator.CreateRandomMultiWordString();
        string settingValueText = $"\'{randomSettingValue}\'";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Project {DataGenerator.CreateRandomString()} " + "{" + settingText + "}";

        ProjectSettingListSyntax columnSettingListClause = ParseProjectSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ProjectSettingListClause);
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
    }
}
