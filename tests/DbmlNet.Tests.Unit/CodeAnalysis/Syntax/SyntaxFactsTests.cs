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
        string? text = SyntaxFacts.GetKnownText(kind);
        Assert.True(text is not null, $"Token text should not be null for the given token kind '{kind}'.");

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

        if (kind == SyntaxKind.BadToken) return skip;
        if (kind == SyntaxKind.WhitespaceTrivia) return skip;
        if (kind == SyntaxKind.EndOfFileToken) return skip;
        if (kind == SyntaxKind.NumberToken) return skip;
        if (kind == SyntaxKind.QuotationMarksStringToken) return skip;
        if (kind == SyntaxKind.SingleQuotationMarksStringToken) return skip;
        if (kind == SyntaxKind.IdentifierToken) return skip;

        return false;
    }
}
