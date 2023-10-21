using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_ColumnDeclaration_With_Name_Identifier()
    {
        SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $"{columnNameText} {columnTypeText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_Name_QuotationMarksString()
    {
        SyntaxKind columnNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomColumnName = DataGenerator.CreateRandomMultiWordString();
        string columnNameText = $"\"{randomColumnName}\"";
        object? columnNameValue = randomColumnName;
        SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $"{columnNameText} {columnTypeText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText, columnNameValue);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_Name_SingleQuotationMarksString()
    {
        SyntaxKind columnNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomColumName = DataGenerator.CreateRandomMultiWordString();
        string columnNameText = $"\'{randomColumName}\'";
        object? columnNameValue = randomColumName;
        SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $"{columnNameText} {columnTypeText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText, columnNameValue);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_ColumnType_Identifier()
    {
        SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $"{columnNameText} {columnTypeText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_ColumnType_QuotationMarksString()
    {
        SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        SyntaxKind columnTypeKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string columnTypeText = $"\"{randomText}\"";
        object columnTypeValue = randomText;
        string text = $"{columnNameText} {columnTypeText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText, columnTypeValue);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_ColumnType_SingleQuotationMarksString()
    {
        SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        SyntaxKind columnTypeKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string columnTypeText = $"\'{randomText}\'";
        object? columnTypeValue = randomText;
        string text = $"{columnNameText} {columnTypeText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText, columnTypeValue);
    }

    [Theory]
    [MemberData(nameof(GetSqlServerColumnTypeIdentifiersData))]
    public void Parse_ColumnDeclaration_With_SqlServer_ColumnTypeIdentifier(
        SyntaxKind columnTypeKind,
        string columnTypeText,
        object? columnTypeValue)
    {
        SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        string text = $"{columnNameText} {columnTypeText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText, columnTypeValue);
    }

    [Theory]
    [MemberData(nameof(GetSqlServerColumnTypeParenthesizedIdentifiersData))]
    public void Parse_ColumnDeclaration_With_SqlServer_ColumnTypeParenthesizedIdentifier(
        SyntaxKind columnTypeIdentifierKind,
        string columnTypeIdentifierText,
        object? columnTypeIdentifierValue,
        SyntaxKind variableLengthIdentifierKind,
        string variableLengthIdentifierText,
        object? variableLengthIdentifierValue)
    {
        SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        string text = $"{columnNameText} {columnTypeIdentifierText}({variableLengthIdentifierText})";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeParenthesizedIdentifierClause);
        e.AssertToken(columnTypeIdentifierKind, columnTypeIdentifierText, columnTypeIdentifierValue);
        e.AssertToken(SyntaxKind.OpenParenthesisToken, "(");
        e.AssertToken(variableLengthIdentifierKind, variableLengthIdentifierText, variableLengthIdentifierValue);
        e.AssertToken(SyntaxKind.CloseParenthesisToken, ")");
    }

    public static IEnumerable<object?[]> GetSqlServerColumnTypeIdentifiersData()
    {
        foreach (string text in SqlServerDataTypes)
        {
            // Skip parenthesized identifiers
            if (text.Contains("("))
                continue;

            yield return new object?[] { SyntaxKind.IdentifierToken, text, null };
        }
    }

    public static IEnumerable<object?[]> GetSqlServerColumnTypeParenthesizedIdentifiersData()
    {
        foreach (string text in SqlServerDataTypes)
        {
            // Skip non parenthesized identifiers
            if (!text.Contains("("))
                continue;

            int indefOfOpenParenthesis = text.IndexOf("(");
            int indefOfCloseParenthesis = text.IndexOf(")");
            SyntaxKind columnTypeIdentifierKind = SyntaxKind.IdentifierToken;
            string columnTypeIdentifierText = text[..indefOfOpenParenthesis];
            object? columnTypeIdentifierValue = null;

            SyntaxKind variableLengthIdentifierKind = SyntaxKind.IdentifierToken;
            string variableLengthIdentifierText = text[(indefOfOpenParenthesis + 1)..indefOfCloseParenthesis];
            object? variableLengthIdentifierValue = null;
            if (decimal.TryParse(variableLengthIdentifierText, out decimal dVal))
            {
                variableLengthIdentifierKind = SyntaxKind.NumberToken;
                variableLengthIdentifierValue = dVal;
            }

            yield return new object?[]
            {
                columnTypeIdentifierKind, columnTypeIdentifierText, columnTypeIdentifierValue,
                variableLengthIdentifierKind, variableLengthIdentifierText, variableLengthIdentifierValue
            };
        }
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_No_Settings()
    {
        SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $"{columnNameText} {columnTypeText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
    }

    [Fact]
    public void Parse_ColumnDeclaration_With_Empty_Settings()
    {
        SyntaxKind columnNameKind = SyntaxKind.IdentifierToken;
        string columnNameText = DataGenerator.CreateRandomString();
        SyntaxKind columnTypeKind = SyntaxKind.IdentifierToken;
        string columnTypeText = DataGenerator.CreateRandomString();
        string text = $"{columnNameText} {columnTypeText} [ ]";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.ColumnDeclarationStatement);
        e.AssertToken(columnNameKind, columnNameText);
        e.AssertNode(SyntaxKind.ColumnTypeIdentifierClause);
        e.AssertToken(columnTypeKind, columnTypeText);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }
}
