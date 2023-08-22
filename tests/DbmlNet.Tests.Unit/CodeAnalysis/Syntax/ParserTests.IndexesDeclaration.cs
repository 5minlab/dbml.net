using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_IndexesDeclaration_With_Empty_Body()
    {
        string text = "indexes { }";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.IndexesDeclarationStatement);
        e.AssertToken(SyntaxKind.IndexesKeyword, "indexes");
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_IndexesDeclaration_With_Index_And_No_Settings()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = CreateRandomString();
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        StatementSyntax statement = ParseStatement(text);

        IndexesDeclarationSyntax indexesDeclarationSyntax =
            Assert.IsAssignableFrom<IndexesDeclarationSyntax>(statement);

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            Assert.IsAssignableFrom<SingleFieldIndexDeclarationSyntax>(
                Assert.Single(indexesDeclarationSyntax.Indexes)
            );

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText);
    }

    [Fact]
    public void Parse_IndexesDeclaration_With_Index_And_Empty_Settings()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = CreateRandomString();
        string indexText = $"{indexNameText} [ ]";
        string text = "indexes {" + indexText + "}";

        StatementSyntax statement = ParseStatement(text);

        IndexesDeclarationSyntax indexesDeclarationSyntax =
            Assert.IsAssignableFrom<IndexesDeclarationSyntax>(statement);

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            Assert.IsAssignableFrom<SingleFieldIndexDeclarationSyntax>(
                Assert.Single(indexesDeclarationSyntax.Indexes)
            );

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_IndexesDeclaration_With_Index_Name_Identifier()
    {
        SyntaxKind indexNameKind = SyntaxKind.IdentifierToken;
        string indexNameText = CreateRandomString();
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        StatementSyntax statement = ParseStatement(text);

        IndexesDeclarationSyntax indexesDeclarationSyntax =
            Assert.IsAssignableFrom<IndexesDeclarationSyntax>(statement);

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            Assert.IsAssignableFrom<SingleFieldIndexDeclarationSyntax>(
                Assert.Single(indexesDeclarationSyntax.Indexes)
            );

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText);
    }

    [Fact(Skip = "Skip to avoid infinite loop.")]
    public void Parse_IndexesDeclaration_With_Index_Name_QuotationMarksString()
    {
        SyntaxKind indexNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string indexNameText = $"\"{randomText}\"";
        object? indexNameValue = randomText;
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        StatementSyntax statement = ParseStatement(text);

        IndexesDeclarationSyntax indexesDeclarationSyntax =
            Assert.IsAssignableFrom<IndexesDeclarationSyntax>(statement);

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            Assert.IsAssignableFrom<SingleFieldIndexDeclarationSyntax>(
                Assert.Single(indexesDeclarationSyntax.Indexes)
            );

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Fact(Skip = "Skip to avoid infinite loop.")]
    public void Parse_IndexesDeclaration_With_Index_Name_SingleQuotationMarksString()
    {
        SyntaxKind indexNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string indexNameText = $"\'{randomText}\'";
        object? indexNameValue = randomText;
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        StatementSyntax statement = ParseStatement(text);

        IndexesDeclarationSyntax indexesDeclarationSyntax =
            Assert.IsAssignableFrom<IndexesDeclarationSyntax>(statement);

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            Assert.IsAssignableFrom<SingleFieldIndexDeclarationSyntax>(
                Assert.Single(indexesDeclarationSyntax.Indexes)
            );

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    [Theory]
    [MemberData(nameof(GetSingleFieldIndexData))]
    public void Parse_IndexesDeclaration_With_SingleFieldIndex(
        SyntaxKind indexNameKind,
        string indexNameText,
        object? indexNameValue)
    {
        string indexText = $"{indexNameText}";
        string text = "indexes {" + indexText + "}";

        StatementSyntax statement = ParseStatement(text);

        IndexesDeclarationSyntax indexesDeclarationSyntax =
            Assert.IsAssignableFrom<IndexesDeclarationSyntax>(statement);

        SingleFieldIndexDeclarationSyntax singleFieldIndexDeclarationSyntax =
            Assert.IsAssignableFrom<SingleFieldIndexDeclarationSyntax>(
                Assert.Single(indexesDeclarationSyntax.Indexes)
            );

        using AssertingEnumerator e = new AssertingEnumerator(singleFieldIndexDeclarationSyntax);
        e.AssertNode(SyntaxKind.SingleFieldIndexDeclarationStatement);
        e.AssertToken(indexNameKind, indexNameText, indexNameValue);
    }

    public static IEnumerable<object?[]> GetSingleFieldIndexData()
    {
        yield return new object?[] { SyntaxKind.IdentifierToken, CreateRandomString(), null };
    }
}
