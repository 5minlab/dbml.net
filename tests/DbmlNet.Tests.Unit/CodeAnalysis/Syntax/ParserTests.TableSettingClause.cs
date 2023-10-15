using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_UnknownTableSettingClause_With_Identifier_Name()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        string settingText = $"{settingNameText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_UnknownTableSettingClause_With_Keyword_Name()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        GetRandomKeyword(
            out SyntaxKind settingNameKind,
            out string settingNameText,
            out object? settingNameValue);
        string settingText = $"{settingNameText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_UnknownTableSettingClause_With_Identifier_Name_And_Identifier_Value()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string randomSettingValue = CreateRandomString();
        string settingValueText = $"{randomSettingValue}";
        object? settingValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_UnknownTableSettingClause_With_Identifier_Name_And_QuotationMarksString_Value()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSettingValue = CreateRandomString();
        string settingValueText = $"\"{randomSettingValue}\"";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_UnknownTableSettingClause_With_Identifier_Name_And_SingleQuotationMarksString_Value()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSettingValue = CreateRandomString();
        string settingValueText = $"\'{randomSettingValue}\'";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"Table {tableNameText} [ {settingText} ]" + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.TableSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownTableSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}