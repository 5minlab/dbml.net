using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_TableDeclaration_With_Name_Identifier()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        string text = $"Table {tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Empty_Body()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        string text = $"Table {tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Note_QuotationMarksString()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        SyntaxKind noteValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomNoteText = CreateRandomMultiWordString();
        string noteValueText = $"\"{randomNoteText}\"";
        object? noteValue = randomNoteText;
        string text = $"Table {tableNameText} {{ note: {noteValueText} }}";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteValueKind, noteValueText, noteValue);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Note_SingleQuotationMarksString()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        SyntaxKind noteValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomNoteText = CreateRandomMultiWordString();
        string noteValueText = $"\'{randomNoteText}\'";
        object? noteValue = randomNoteText;
        string text = $"Table {tableNameText} {{ note: {noteValueText} }}";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.NoteDeclarationStatement);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(noteValueKind, noteValueText, noteValue);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
