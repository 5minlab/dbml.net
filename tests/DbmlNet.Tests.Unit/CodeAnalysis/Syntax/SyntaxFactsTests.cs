using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public class SyntaxFactsTests
{
    [Theory]
    [MemberData(nameof(GetSyntaxKindData), DisableDiscoveryEnumeration = true)]
    public void SyntaxFacts_GetText_Returned_Text_Should_Parse_Same_TokenKind(SyntaxKind kind)
    {
        string? text = kind.GetKnownText();
        string message = $"""
        Invalid input, expected token text not be null for kind '{kind}', did you forget to update `SyntaxFacts.GetKnownText(kind)`?
        """;
        Assert.True(text is not null, message);

        ImmutableArray<SyntaxToken> tokens = SyntaxTree.ParseTokens(text);
        SyntaxToken token = Assert.Single(tokens);
        Assert.Equal(kind, token.Kind);
        Assert.Equal(text, token.Text);
    }

    public static IEnumerable<object[]> GetSyntaxKindData()
    {
        foreach (SyntaxKind kind in Enum.GetValues<SyntaxKind>())
        {
            if (!SkipTextVerificationForTokenKind(kind))
                yield return new object[] { kind };
        }
    }

    private static bool SkipTextVerificationForTokenKind(SyntaxKind kind)
    {
        const bool skip = true;

        if (kind.IsSyntaxMember()) return skip;
        if (kind.IsSyntaxStatement()) return skip;
        if (kind.IsSyntaxExpression()) return skip;

        if (kind.IsTrivia()) return skip;
        if (kind.IsStringToken()) return skip;
        if (kind == SyntaxKind.BadToken) return skip;
        if (kind == SyntaxKind.WhitespaceTrivia) return skip;
        if (kind == SyntaxKind.EndOfFileToken) return skip;
        if (kind == SyntaxKind.NumberToken) return skip;
        if (kind == SyntaxKind.IdentifierToken) return skip;

        return !skip;
    }
}
