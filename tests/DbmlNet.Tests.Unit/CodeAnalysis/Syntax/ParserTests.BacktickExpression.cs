using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_BacktickExpression_With_NameExpression()
    {
        SyntaxKind identifierKind = SyntaxKind.IdentifierToken;
        string randomIdentifierText = CreateRandomString();
        string identifierText = $"{randomIdentifierText}";
        object? identifierValue = null;
        string text = $"`{identifierText}`";

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.BacktickExpression);
        e.AssertToken(SyntaxKind.BacktickToken, "`");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(identifierKind, identifierText, identifierValue);
        e.AssertToken(SyntaxKind.BacktickToken, "`");
    }
}
