using System.Collections.Generic;
using System.Linq;

using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_CallExpression()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = DataGenerator.CreateRandomString();
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
        string functionNameText = DataGenerator.CreateRandomString();
        SyntaxKind argKind = SyntaxKind.IdentifierToken;
        string argRandomValue = DataGenerator.CreateRandomString();
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
    public void Parse_CallExpression_With_Multiple_Arguments()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = DataGenerator.CreateRandomString();
        List<(SyntaxKind Kind, string Text, object? Value)> arguments = new();
        int randomNumberOfArguments = DataGenerator.GetRandomNumber(min: 0, max: 10);
        for (int i = 0; i < randomNumberOfArguments; i++)
        {
            SyntaxKind argKind = SyntaxKind.IdentifierToken;
            string argRandomValue = DataGenerator.CreateRandomString();
            string argText = $"{argRandomValue}";
            object? argValue = null;
            arguments.Add((argKind, argText, argValue));
        }

        string argsText = string.Join(", ", arguments.Select(arg => arg.Text));
        string text = $"{functionNameText} ( {argsText} ) ";

        ExpressionSyntax expression = ParseExpression(text);

        using AssertingEnumerator e = new AssertingEnumerator(expression);
        e.AssertNode(SyntaxKind.CallExpression);
        e.AssertToken(functionNameKind, functionNameText);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        foreach ((SyntaxKind argKind, string argText, object? argValue) in arguments)
        {
            e.AssertNode(SyntaxKind.NameExpression);
            e.AssertToken(argKind, argText, argValue);
        }

        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    [Fact]
    public void Parse_CallExpression_With_Identifier_Argument()
    {
        SyntaxKind functionNameKind = SyntaxKind.IdentifierToken;
        string functionNameText = DataGenerator.CreateRandomString();
        SyntaxKind argKind = SyntaxKind.IdentifierToken;
        string argRandomValue = DataGenerator.CreateRandomString();
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
        string functionNameText = DataGenerator.CreateRandomString();
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
        string functionNameText = DataGenerator.CreateRandomString();
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
        string functionNameText = DataGenerator.CreateRandomString();
        SyntaxKind argKind = SyntaxKind.NumberToken;
        decimal argRandomNumber = DataGenerator.GetRandomNumber(min: 0, max: 10);
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
        string functionNameText = DataGenerator.CreateRandomString();
        SyntaxKind argKind = SyntaxKind.QuotationMarksStringToken;
        string argRandomValue = DataGenerator.CreateRandomMultiWordString();
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
        string functionNameText = DataGenerator.CreateRandomString();
        SyntaxKind argKind = SyntaxKind.SingleQuotationMarksStringToken;
        string argRandomValue = DataGenerator.CreateRandomMultiWordString();
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
