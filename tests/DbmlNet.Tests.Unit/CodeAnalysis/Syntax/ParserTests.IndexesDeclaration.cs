using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_IndexesDeclaration_With_Empty_Body()
    {
        string text = "indexes { }";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.IndexesDeclarationStatement);
        e.AssertToken(SyntaxKind.IndexesKeyword, "indexes");
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_IndexesDeclaration_With_SingleFieldIndexDeclaration()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
        string indexNameText = randomText;
        object? indexNameValue = null;
        string indexText = $"{indexNameText}";
        string text = "indexes { " + indexText + " }";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.IndexesDeclarationStatement);
        e.AssertToken(SyntaxKind.IndexesKeyword, "indexes");
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_IndexesDeclaration_With_CompositeIndexDeclaration()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = DataGenerator.CreateRandomString();
        string indexText = "(" + indexNameText + ")";
        string text = "indexes { " + indexText + " }";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.IndexesDeclarationStatement);
        e.AssertToken(SyntaxKind.IndexesKeyword, "indexes");
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.CompositeIndexDeclarationStatement);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(indexNameKind, indexNameText);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
