using System;
using System.Linq;

using DbmlNet.CodeAnalysis.Syntax;

using Tynamix.ObjectFiller;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public class SyntaxTokenTests
{
    [Fact]
    public void SyntaxToken_Constructor_Should_Set_Properties_With_Given_Input()
    {
        SyntaxKind expectedKind = GetRandomSyntaxKind();
        int expectedStart = GetRandomNumber();
        string expectedText = CreateRandomString();
        object? expectedValue = CreateRandomString();

        SyntaxToken token =
            new SyntaxToken(syntaxTree: null!, expectedKind, expectedStart, expectedText, expectedValue);

        Assert.Equal(expectedKind, token.Kind);
        Assert.Equal(expectedStart, token.Start);
        Assert.Equal(expectedText, token.Text);
        Assert.Equal(expectedValue, token.Value);
    }

    [Fact]
    public void SyntaxToken_Length_Should_Return_Expected_LengthValue()
    {
        SyntaxKind expectedKind = GetRandomSyntaxKind();
        int expectedStart = GetRandomNumber();
        string expectedText = CreateRandomString();
        int expectedEnd = expectedText.Length;

        SyntaxToken token =
            new SyntaxToken(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.Equal(expectedEnd, token.Length);
    }

    [Fact]
    public void SyntaxToken_End_Should_Return_Expected_EndValue()
    {
        SyntaxKind expectedKind = GetRandomSyntaxKind();
        int expectedStart = GetRandomNumber();
        string expectedText = CreateRandomString();
        int expectedEnd = expectedStart + expectedText.Length;

        SyntaxToken token =
            new SyntaxToken(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.Equal(expectedEnd, token.End);
    }

    [Fact]
    public void SyntaxToken_IsMissing_Should_Return_True_For_Null_TokenText()
    {
        SyntaxKind expectedKind = GetRandomSyntaxKind();
        int expectedStart = GetRandomNumber();
        string? expectedText = null;

        SyntaxToken token =
            new SyntaxToken(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.True(token.IsMissing, "Token should be missing.");
    }

    [Fact]
    public void SyntaxToken_IsMissing_Should_Return_False_For_Valid_TokenText()
    {
        SyntaxKind expectedKind = GetRandomSyntaxKind();
        int expectedStart = GetRandomNumber();
        string expectedText = CreateRandomString();

        SyntaxToken token =
            new SyntaxToken(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.False(token.IsMissing, "Token should not be missing.");
    }

    [Fact]
    public void SyntaxToken_ToString_Should_Return_TokenText()
    {
        SyntaxKind expectedKind = GetRandomSyntaxKind();
        int expectedStart = GetRandomNumber();
        string expectedText = CreateRandomString();

        SyntaxToken token =
            new SyntaxToken(syntaxTree: null!, expectedKind, expectedStart, expectedText);

        Assert.Equal(expectedText, token.ToString());
    }

    [Fact]
    public void SyntaxToken_GetChildren_Should_Always_Return_Empty_List()
    {
        SyntaxKind expectedKind = GetRandomSyntaxKind();
        int expectedStart = GetRandomNumber();
        string expectedText = CreateRandomString();
        object? expectedValue = CreateRandomString();

        SyntaxToken token =
            new SyntaxToken(syntaxTree: null!, expectedKind, expectedStart, expectedText, expectedValue);

        Assert.Empty(token.GetChildren());
    }

    private static SyntaxKind GetRandomSyntaxKind()
    {
        int min = Enum.GetValues<SyntaxKind>().Min(kind => (int)kind);
        int max = Enum.GetValues<SyntaxKind>().Max(kind => (int)kind);
        int randomNumber = new IntRange(min, max).GetValue();

        return Enum.TryParse($"{randomNumber}", out SyntaxKind randomKind)
            ? randomKind
            : throw new Exception($"ERROR: Cannot generate random SyntaxKind from <{randomNumber}>.");
    }

    private static int GetRandomNumber() =>
        new IntRange(min: 0, max: int.MaxValue).GetValue();

    private static string CreateRandomString() =>
        new MnemonicString().GetValue();
}
