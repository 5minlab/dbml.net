using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_BlockStatement_With_Empty_Block()
    {
        string expectedText = "{}";

        StatementSyntax statement = ParseStatement(expectedText);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_BlockStatement_With_Statement()
    {
        string randomText = CreateRandomString();
        string expectedText = "{" + randomText + "}";

        StatementSyntax statement = ParseStatement(expectedText);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.ExpressionStatement);
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(SyntaxKind.IdentifierToken, randomText);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
