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
}
