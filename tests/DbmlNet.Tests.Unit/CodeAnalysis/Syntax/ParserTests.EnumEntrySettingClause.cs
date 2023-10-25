using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_NoteEnumEntrySettingClause_With_QuotationMarksString_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"\"{randomText}\"";
        object? settingValue = randomText;
        string text = $$"""
        {{DataGenerator.CreateRandomString()}} [ note: {{settingText}} ]
        """;

        EnumEntrySettingListSyntax enumEntrySettingListClause =
            ParseEnumEntrySettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(enumEntrySettingListClause);
        e.AssertNode(SyntaxKind.EnumEntrySettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteEnumEntrySettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NoteEnumEntrySettingClause_With_SingleQuotationMarksString_Value()
    {
        const SyntaxKind settingKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string settingText = $"\'{randomText}\'";
        object? settingValue = randomText;
        string text = $$"""
        {{DataGenerator.CreateRandomString()}} [ note: {{settingText}} ]
        """;

        EnumEntrySettingListSyntax enumEntrySettingListClause =
            ParseEnumEntrySettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(enumEntrySettingListClause);
        e.AssertNode(SyntaxKind.EnumEntrySettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteEnumEntrySettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownEnumEntrySettingClause_With_Simple_Setting()
    {
        const SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        object? settingValue = null;
        string text = $$"""
        {{DataGenerator.CreateRandomString()}} [ {{settingNameText}} ]
        """;
        string[] diagnosticMessages = new[]
        {
            $"Unknown enum entry setting '{settingNameText}'.",
        };

        EnumEntrySettingListSyntax enumEntrySettingListClause =
            ParseEnumEntrySettingListClause(text, diagnosticMessages);

        using AssertingEnumerator e = new AssertingEnumerator(enumEntrySettingListClause);
        e.AssertNode(SyntaxKind.EnumEntrySettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownEnumEntrySettingClause);
        e.AssertToken(settingKind, settingNameText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
