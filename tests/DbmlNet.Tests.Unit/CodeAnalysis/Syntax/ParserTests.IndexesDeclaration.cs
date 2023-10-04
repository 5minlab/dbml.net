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
}
