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
    public void Parse_CallExpression_With_Single_Argument()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = CreateRandomString();
        SyntaxKind argKind = SyntaxKind.IdentifierToken;
        string argRandomValue = CreateRandomString();
        string argText = $"{argRandomValue}";
        object? argValue = null;
        string text = $"{functionNameText} ( {argText} ) ";

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.CallExpression);
        e.AssertToken(functionNameKind, functionNameText);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(argKind, argText, argValue);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_CallExpression_With_Identifier_Argument()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = CreateRandomString();
        SyntaxKind argKind = SyntaxKind.IdentifierToken;
        string argRandomValue = CreateRandomString();
        string argText = $"{argRandomValue}";
        object? argValue = null;
        string text = $"{functionNameText} ( {argText} ) ";

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.CallExpression);
        e.AssertToken(functionNameKind, functionNameText);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(argKind, argText, argValue);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_CallExpression_With_Boolean_False_Argument()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = CreateRandomString();
        SyntaxKind argKind = SyntaxKind.FalseKeyword;
        string argText = "false";
        object? argValue = false;
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

    [Fact]
    public void Parse_CallExpression_With_Boolean_True_Argument()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = CreateRandomString();
        SyntaxKind argKind = SyntaxKind.TrueKeyword;
        string argText = "true";
        object? argValue = true;
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

    [Fact]
    public void Parse_CallExpression_With_Number_Argument()
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

    [Fact]
    public void Parse_CallExpression_With_QuotationMarksString_Argument()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = CreateRandomString();
        SyntaxKind argKind = SyntaxKind.QuotationMarksStringToken;
        string argRandomValue = CreateRandomMultiWordString();
        string argText = $"\"{argRandomValue}\"";
        object? argValue = argRandomValue;
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

    [Fact]
    public void Parse_CallExpression_With_SingleQuotationMarksString_Argument()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = CreateRandomString();
        SyntaxKind argKind = SyntaxKind.SingleQuotationMarksStringToken;
        string argRandomValue = CreateRandomMultiWordString();
        string argText = $"\'{argRandomValue}\'";
        object? argValue = argRandomValue;
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
