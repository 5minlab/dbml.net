using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_NoteDeclaration_With_QuotationMarksString()
    {
        SyntaxKind noteKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string noteText = $"\"{randomText}\"";
        string text = $"note: {noteText}";
        object noteValue = randomText;

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteKind, noteText, noteValue);
    }

    [Fact]
    public void Parse_NoteDeclaration_With_SingleQuotationMarksString()
    {
        SyntaxKind noteKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string noteText = $"\'{randomText}\'";
        object noteValue = randomText;
        string text = $"note: {noteText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteKind, noteText, noteValue);
    }
}
