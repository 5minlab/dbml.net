using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_NullExpression()
    {
        SyntaxKind expectedKind = SyntaxKind.NullKeyword;
        string expectedText = "null";

        ExpressionSyntax expression = ParseExpression(expectedText);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.NullExpression);
        e.AssertToken(expectedKind, expectedText);
    }
}
