using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_BacktickExpression_With_NameExpression()
    {
        SyntaxKind identifierKind = SyntaxKind.IdentifierToken;
        string randomIdentifierName = CreateRandomString();
        string identifierText = $"{randomIdentifierName}";
        object? identifierValue = null;
        string text = $"`{identifierText}`";

        BacktickExpressionSyntax expression = ParseBacktickExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression.Expression);
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(identifierKind, identifierText, identifierValue);
    }

    [Fact]
    public void Parse_BacktickExpression_With_CallExpression()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string randomFunctionName = CreateRandomString();
        string functionNameText = $"{randomFunctionName}";
        object? functionNameValue = null;
        string expressionText = $"{randomFunctionName}()";
        string text = $"`{expressionText}`";

        BacktickExpressionSyntax expression = ParseBacktickExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression.Expression);
        e.AssertNode(SyntaxKind.CallExpression);
        e.AssertToken(functionNameKind, functionNameText, functionNameValue);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }
}
