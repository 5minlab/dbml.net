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
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_QuotationMarksString()
    {
        SyntaxKind tableNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomString();
        string tableNameText = $"\"{randomText}\"";
        object? tableNameValue = randomText;
        string text = $"Table {tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_SingleQuotationMarksString()
    {
        SyntaxKind tableNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomString();
        string tableNameText = $"\'{randomText}\'";
        object? tableNameValue = randomText;
        string text = $"Table {tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
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
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
