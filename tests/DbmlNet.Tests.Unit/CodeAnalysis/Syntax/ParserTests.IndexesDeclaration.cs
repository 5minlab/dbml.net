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
    public void Parse_IndexesDeclaration_With_Index_Identifier()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
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
}
