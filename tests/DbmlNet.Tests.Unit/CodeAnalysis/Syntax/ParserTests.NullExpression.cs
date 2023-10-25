using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_NullExpression()
    {
        const SyntaxKind expectedKind = SyntaxKind.NullKeyword;
        const string expectedText = "null";

        ExpressionSyntax expression = ParseExpression(expectedText);

        using AssertingEnumerator e = new(expression);
        e.AssertNode(SyntaxKind.NullExpression);
        e.AssertToken(expectedKind, expectedText);
    }
}
