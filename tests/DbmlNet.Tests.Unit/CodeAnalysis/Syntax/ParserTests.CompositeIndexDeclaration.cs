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
    public void Parse_CompositeIndexDeclaration_With_Two_Indexes_Identifier_Name()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string firstIndexNameText = CreateRandomString();
        string secondIndexNameText = CreateRandomString();
        string indexText = $"( {firstIndexNameText}, {secondIndexNameText})";
        string text = "indexes { " + indexText + " }";

        CompositeIndexDeclarationSyntax compositeFieldIndexDeclarationSyntax =
            ParseCompositeIndexDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(compositeFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(indexNameKind, firstIndexNameText);
        e.AssertToken(SyntaxKind.CommaToken, ",");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(indexNameKind, secondIndexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }
}
