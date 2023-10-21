using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_CompositeIndexDeclaration_With_One_Index_Identifier_Name()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        string indexText = "(" + indexNameText + ")";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(indexNameKind, indexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_One_Index_No_Settings()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        string indexText = "(" + indexNameText + ")";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(indexNameKind, indexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_One_Index_Empty_Settings()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        string indexText = "(" + indexNameText + ") [ ]";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(indexNameKind, indexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_One_Index_Unknown_Setting()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingName = DataGenerator.CreateRandomString();
        string indexText = "(" + indexNameText + ") [ " + settingName + " ]";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(indexNameKind, indexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownIndexSettingClause);
        e.AssertToken(settingNameKind, settingName);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_Two_Indexes_Identifier_Name()
    {
        SyntaxKind firstIndexNameKind = SyntaxKind.IdentifierToken;
        string firstIndexNameText = DataGenerator.CreateRandomString();
        SyntaxKind secondIndexNameKind = SyntaxKind.IdentifierToken;
        string secondIndexNameText = DataGenerator.CreateRandomString();
        string indexText = $"( {firstIndexNameText}, {secondIndexNameText})";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(firstIndexNameKind, firstIndexNameText);
        e.AssertToken(SyntaxKind.CommaToken, ",");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(secondIndexNameKind, secondIndexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_Two_Indexes_No_Settings()
    {
        SyntaxKind firstIndexNameKind = SyntaxKind.IdentifierToken;
        string firstIndexNameText = DataGenerator.CreateRandomString();
        SyntaxKind secondIndexNameKind = SyntaxKind.IdentifierToken;
        string secondIndexNameText = DataGenerator.CreateRandomString();
        string indexText = $"( {firstIndexNameText}, {secondIndexNameText})";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(firstIndexNameKind, firstIndexNameText);
        e.AssertToken(SyntaxKind.CommaToken, ",");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(secondIndexNameKind, secondIndexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_Two_Indexes_Empty_Settings()
    {
        SyntaxKind firstIndexNameKind = SyntaxKind.IdentifierToken;
        string firstIndexNameText = DataGenerator.CreateRandomString();
        SyntaxKind secondIndexNameKind = SyntaxKind.IdentifierToken;
        string secondIndexNameText = DataGenerator.CreateRandomString();
        string indexText = $"( {firstIndexNameText}, {secondIndexNameText}) [ ]";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(firstIndexNameKind, firstIndexNameText);
        e.AssertToken(SyntaxKind.CommaToken, ",");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(secondIndexNameKind, secondIndexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_Two_Indexes_Unknown_Setting()
    {
        SyntaxKind firstIndexNameKind = SyntaxKind.IdentifierToken;
        string firstIndexNameText = DataGenerator.CreateRandomString();
        SyntaxKind secondIndexNameKind = SyntaxKind.IdentifierToken;
        string secondIndexNameText = DataGenerator.CreateRandomString();
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        string indexText = $"( {firstIndexNameText}, {secondIndexNameText}) [ " + settingNameText + " ]";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(firstIndexNameKind, firstIndexNameText);
        e.AssertToken(SyntaxKind.CommaToken, ",");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(secondIndexNameKind, secondIndexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
        e.AssertNode(SyntaxKind.IndexSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownIndexSettingClause);
        e.AssertToken(settingNameKind, settingNameText);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
