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
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Type_Settings_Identifier_Value()
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
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.IndexSettingExpression);
        e.AssertToken(SyntaxKind.TypeKeyword, "type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_SingleFieldIndexDeclaration_With_Type_Settings_QuotationMarksStringToken_Value()
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
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.IndexSettingExpression);
        e.AssertToken(SyntaxKind.TypeKeyword, "type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
