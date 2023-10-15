using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_CallExpression()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = CreateRandomString();
        string functionCallText = $"{functionNameText}()";
        string text = $"{functionCallText}";

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.CallExpression);
        e.AssertToken(functionNameKind, functionNameText);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_CallExpression_With_NumberLiteral_Argument()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = CreateRandomString();
        SyntaxKind argKind = SyntaxKind.NumberToken;
        decimal argRandomNumber = GetRandomNumber();
        string argText = $"{argRandomNumber}";
        object? argValue = argRandomNumber;
        string text = $"{functionNameText} ( {argText} ) ";

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.CallExpression);
        e.AssertToken(functionNameKind, functionNameText);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(argKind, argText, argValue);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }
}
