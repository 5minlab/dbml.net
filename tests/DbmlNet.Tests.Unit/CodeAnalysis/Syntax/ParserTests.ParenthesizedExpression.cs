using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_ParenthesizedExpression_With_No_Expression()
    {
        SyntaxKind expectedKind = SyntaxKind.IdentifierToken;
        string expectedText = string.Empty;
        string text = "()";
        object? expectedValue = null;

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.ParenthesizedExpression);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(expectedKind, expectedText, expectedValue, isMissing: true);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_ParenthesizedExpression_With_Expression()
    {
        SyntaxKind expectedKind = SyntaxKind.NumberToken;
        decimal randomNumber = DataGenerator.GetRandomDecimal(min: 0);
        string expectedText = $"{randomNumber}";
        object? expectedValue = randomNumber;
        string text = "(" + expectedText + ")";

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.ParenthesizedExpression);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(expectedKind, expectedText, expectedValue);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }
}
