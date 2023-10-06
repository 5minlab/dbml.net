using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Identifier_Name()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        string indexText = $"{indexNameText}";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact(Skip = "Skip to avoid infinite loop.")]
    public void Parse_SingleFieldIndexDeclaration_With_QuotationMarksString_Name()
    {
        SyntaxKind indexNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string indexNameText = $"\"{randomText}\"";
        object? indexNameValue = randomText;
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact(Skip = "Skip to avoid infinite loop.")]
    public void Parse_SingleFieldIndexDeclaration_With_SingleQuotationMarksString_Name()
    {
        SyntaxKind indexNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string indexNameText = $"\'{randomText}\'";
        object? indexNameValue = randomText;
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_No_Settings()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        string indexText = $"{indexNameText}";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Empty_Settings()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Unique_Setting()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ unique ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UniqueIndexSettingClause);
        e.AssertToken(SyntaxKind.UniqueKeyword, "unique");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Pk_Setting()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ pk ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.PkIndexSettingClause);
        e.AssertToken(SyntaxKind.PkKeyword, "pk");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_PrimaryKey_Setting()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        string indexText = $"{indexNameText} [ primary key ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.PrimaryKeyIndexSettingClause);
        e.AssertToken(SyntaxKind.PrimaryKeyword, "primary");
        e.AssertToken(SyntaxKind.KeyKeyword, "key");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact(Skip = "Skip to avoid infinite loop.")]
    public void Parse_SingleFieldIndexDeclaration_With_Name_Setting_Identifier_Value()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = CreateRandomString();
        object? settingValue = null;
        string indexText = $"{indexNameText} [ name: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NameIndexSettingClause);
        e.AssertToken(SyntaxKind.NameKeyword, "name");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Name_Setting_QuotationMarksStringToken_Value()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSetting = CreateRandomString();
        string settingValueText = $"\"{randomSetting}\"";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ name: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NameIndexSettingClause);
        e.AssertToken(SyntaxKind.NameKeyword, "name");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Name_Setting_SingleQuotationMarksStringToken_Value()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSetting = CreateRandomString();
        string settingValueText = $"\'{randomSetting}\'";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ name: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NameIndexSettingClause);
        e.AssertToken(SyntaxKind.NameKeyword, "name");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Type_Setting_Identifier_Value()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string settingValueText = CreateRandomString();
        object? settingValue = null;
        string indexText = $"{indexNameText} [ type: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.TypeIndexSettingClause);
        e.AssertToken(SyntaxKind.TypeKeyword, "type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Type_Setting_QuotationMarksStringToken_Value()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSetting = CreateRandomString();
        string settingValueText = $"\"{randomSetting}\"";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ type: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.TypeIndexSettingClause);
        e.AssertToken(SyntaxKind.TypeKeyword, "type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Type_Setting_SingleQuotationMarksStringToken_Value()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSetting = CreateRandomString();
        string settingValueText = $"\'{randomSetting}\'";
        object? settingValue = randomSetting;
        string indexText = $"{indexNameText} [ type: {settingValueText} ]";
        string text = "indexes { " + indexText + " }";

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            ParseSingleFieldIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.TypeIndexSettingClause);
        e.AssertToken(SyntaxKind.TypeKeyword, "type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
