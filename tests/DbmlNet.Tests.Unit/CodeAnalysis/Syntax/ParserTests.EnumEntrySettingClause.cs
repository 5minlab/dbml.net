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
}
