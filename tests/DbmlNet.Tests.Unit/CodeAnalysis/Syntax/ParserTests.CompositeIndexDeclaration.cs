using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_CompositeIndexDeclaration_With_One_Index_Identifier_Name()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = CreateRandomString();
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
        string indexNameText = CreateRandomString();
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
        string indexNameText = CreateRandomString();
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
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_One_Index_Unknown_Setting()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = CreateRandomString();
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingName = CreateRandomString();
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
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.IndexSettingExpression);
        e.AssertToken(settingNameKind, settingName);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_CompositeIndexDeclaration_With_Two_Indexes_Identifier_Name()
    {
        SyntaxKind firstIndexNameKind = SyntaxKind.IdentifierToken;
        string firstIndexNameText = CreateRandomString();
        SyntaxKind secondIndexNameKind = SyntaxKind.IdentifierToken;
        string secondIndexNameText = CreateRandomString();
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
}
