using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_TableDeclaration_With_Name_Identifier()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
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

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_TableDeclaration_With_Name_Keyword(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        string text = $"Table {keywordText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_And_Schema_Identifier()
    {
        SyntaxKind schemaNameKind = SyntaxKind.IdentifierToken;
        string randomSchemaName = DataGenerator.CreateRandomString();
        string schemaNameText = randomSchemaName;
        object? schemaNameValue = null;
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomTableName = DataGenerator.CreateRandomString();
        string tableNameText = randomTableName;
        object? tableNameValue = null;
        string text = $"Table {schemaNameText}.{tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(schemaNameKind, schemaNameText, schemaNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_TableDeclaration_With_Name_And_Schema_Keyword(
        SyntaxKind keywordKind,
        string keywordText,
        object? keywordValue)
    {
        string text = $"Table {keywordText}.{keywordText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(keywordKind, keywordText, keywordValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Name_And_Schema_And_Database_Identifier()
    {
        SyntaxKind databaseNameKind = SyntaxKind.IdentifierToken;
        string randomDatabaseName = DataGenerator.CreateRandomString();
        string databaseNameText = randomDatabaseName;
        object? databaseNameValue = null;
        SyntaxKind schemaNameKind = SyntaxKind.IdentifierToken;
        string randomSchemaName = DataGenerator.CreateRandomString();
        string schemaNameText = randomSchemaName;
        object? schemaNameValue = null;
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomTableName = DataGenerator.CreateRandomString();
        string tableNameText = randomTableName;
        object? tableNameValue = null;
        string text = $"Table {databaseNameText}.{schemaNameText}.{tableNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.TableDeclarationMember);
        e.AssertToken(SyntaxKind.TableKeyword, "Table");
        e.AssertNode(SyntaxKind.TableIdentifierClause);
        e.AssertToken(databaseNameKind, databaseNameText, databaseNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(schemaNameKind, schemaNameText, schemaNameValue);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_TableDeclaration_With_Empty_Body()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
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
        string randomText = DataGenerator.CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        SyntaxKind noteValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomNoteText = DataGenerator.CreateRandomMultiWordString();
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
        string randomText = DataGenerator.CreateRandomString();
        string tableNameText = randomText;
        object? tableNameValue = null;
        SyntaxKind noteValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomNoteText = DataGenerator.CreateRandomMultiWordString();
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
