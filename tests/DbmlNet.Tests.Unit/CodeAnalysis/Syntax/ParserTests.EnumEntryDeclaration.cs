using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_EnumEntryDeclaration_With_Name_Identifier()
    {
        const SyntaxKind enumEntryNameKind = SyntaxKind.IdentifierToken;
        string enumEntryNameText = DataGenerator.CreateRandomString();
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}}
        }
        """;

        StatementSyntax statement = ParseEnumEntryDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.EnumEntryDeclarationStatement);
        e.AssertToken(enumEntryNameKind, enumEntryNameText);
    }

    [Fact]
    public void Parse_EnumEntryDeclaration_With_Name_QuotationMarksString()
    {
        const SyntaxKind enumEntryNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomEnumEntryName = DataGenerator.CreateRandomMultiWordString();
        string enumEntryNameText = $"\"{randomEnumEntryName}\"";
        object? enumEntryNameValue = randomEnumEntryName;
        string text = $$"""
        enum {{DataGenerator.CreateRandomString()}}
        {
            {{enumEntryNameText}}
        }
        """;

        StatementSyntax statement = ParseEnumEntryDeclaration(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.EnumEntryDeclarationStatement);
        e.AssertToken(enumEntryNameKind, enumEntryNameText, enumEntryNameValue);
    }
}
