using System.Collections.Immutable;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_PrimaryKeyColumnSettingClause()
    {
        string text = $"{CreateRandomString()} {CreateRandomString()} [ primary key ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.PrimaryKeyColumnSettingClause);
        e.AssertToken(SyntaxKind.PrimaryKeyword, "primary");
        e.AssertToken(SyntaxKind.KeyKeyword, "key");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_PkColumnSettingClause()
    {
        string text = $"{CreateRandomString()} {CreateRandomString()} [ pk ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.PkColumnSettingClause);
        e.AssertToken(SyntaxKind.PkKeyword, "pk");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NullColumnSettingClause()
    {
        string text = $"{CreateRandomString()} {CreateRandomString()} [ null ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NullColumnSettingClause);
        e.AssertToken(SyntaxKind.NullKeyword, "null");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NotNullColumnSettingClause()
    {
        string text = $"{CreateRandomString()} {CreateRandomString()} [ not null ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NotNullColumnSettingClause);
        e.AssertToken(SyntaxKind.NotKeyword, "not");
        e.AssertToken(SyntaxKind.NullKeyword, "null");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UniqueColumnSettingClause()
    {
        string text = $"{CreateRandomString()} {CreateRandomString()} [ unique ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UniqueColumnSettingClause);
        e.AssertToken(SyntaxKind.UniqueKeyword, "unique");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_IncrementColumnSettingClause()
    {
        string text = $"{CreateRandomString()} {CreateRandomString()} [ increment ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.IncrementColumnSettingClause);
        e.AssertToken(SyntaxKind.IncrementKeyword, "increment");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NoteColumnSettingClause_With_QuotationMarksString_Value()
    {
        SyntaxKind settingKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string settingText = $"\"{randomText}\"";
        object? settingValue = randomText;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ note: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteColumnSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_NoteColumnSettingClause_With_SingleQuotationMarksString_Value()
    {
        SyntaxKind settingKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string settingText = $"\'{randomText}\'";
        object? settingValue = randomText;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ note: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.NoteColumnSettingClause);
        e.AssertToken(SyntaxKind.NoteKeyword, "note");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Identifier_Value()
    {
        SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string settingText = randomText;
        object? settingValue = null;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Number_Value()
    {
        SyntaxKind settingKind = SyntaxKind.NumberToken;
        decimal randomNumber = GetRandomDecimal();
        string settingText = $"{randomNumber}";
        object? settingValue = randomNumber;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_DecimalPointNumber_Value()
    {
        SyntaxKind settingKind = SyntaxKind.NumberToken;
        string settingText = $"{GetRandomNumber()}.{GetRandomNumber()}";
        object? settingValue = decimal.TryParse(settingText, out decimal dVal) ? dVal : null;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Bool_False_Value()
    {
        SyntaxKind settingKind = SyntaxKind.FalseKeyword;
        string settingText = $"false";
        object? settingValue = false;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Bool_True_Value()
    {
        SyntaxKind settingKind = SyntaxKind.TrueKeyword;
        string settingText = $"true";
        object? settingValue = true;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_Null_Value()
    {
        SyntaxKind settingKind = SyntaxKind.NullKeyword;
        string settingText = $"null";
        object? settingValue = null;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.NullExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_QuotationMarksString_Value()
    {
        SyntaxKind settingKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string settingText = $"\"{randomText}\"";
        object? settingValue = randomText;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_SingleQuotationMarksString_Value()
    {
        SyntaxKind settingKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string settingText = $"\'{randomText}\'";
        object? settingValue = randomText;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.LiteralExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_DefaultColumnSettingClause_With_NameExpression_Value()
    {
        SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string identifierExpressionText = $"{CreateRandomString()}";
        string settingText = $"{identifierExpressionText}";
        object? settingValue = null;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ default: `{settingText}` ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.DefaultColumnSettingClause);
        e.AssertToken(SyntaxKind.DefaultKeyword, "default");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.BacktickExpression);
        e.AssertToken(SyntaxKind.BacktickToken, "`");
        e.AssertNode(SyntaxKind.NameExpression);
        e.AssertToken(settingKind, settingText, settingValue);
        e.AssertToken(SyntaxKind.BacktickToken, "`");
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Warning_Diagnostic()
    {
        string randomText = CreateRandomString();
        string settingNameText = randomText;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ {settingNameText} ]";

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);

        Diagnostic diagnostic = Assert.Single(diagnostics);
        string expectedDiagnosticMessage = $"Unknown column setting '{settingNameText}'.";
        Assert.Equal(expectedDiagnosticMessage, diagnostic.Message);
        Assert.Equal(expectedDiagnosticMessage, $"{diagnostic}");
        Assert.True(diagnostic.IsWarning, "Diagnostic should be warning.");
        Assert.False(diagnostic.IsError, "Diagnostic should not be error.");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Simple_Setting()
    {
        SyntaxKind settingKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string settingNameText = randomText;
        object? settingValue = null;
        string text = $"{CreateRandomString()} {CreateRandomString()} [ {settingNameText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingKind, settingNameText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Composed_Setting_Identifier_Value()
    {
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.IdentifierToken;
        string randomSettingValue = CreateRandomString();
        string settingValueText = randomSettingValue;
        object? settingValue = null;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"{CreateRandomString()} {CreateRandomString()} [ {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Composed_Setting_QuotationMarksString_Value()
    {
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.QuotationMarksStringToken;
        string randomSettingValue = CreateRandomMultiWordString();
        string settingValueText = $"\"{randomSettingValue}\"";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"{CreateRandomString()} {CreateRandomString()} [ {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Fact]
    public void Parse_UnknownColumnSettingClause_With_Composed_Setting_SingleQuotationMarksString_Value()
    {
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string randomSettingName = CreateRandomString();
        string settingNameText = randomSettingName;
        object? settingNameValue = null;
        SyntaxKind settingValueKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomSettingValue = CreateRandomMultiWordString();
        string settingValueText = $"\'{randomSettingValue}\'";
        object? settingValue = randomSettingValue;
        string settingText = $"{settingNameText}: {settingValueText}";
        string text = $"{CreateRandomString()} {CreateRandomString()} [ {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.UnknownColumnSettingClause);
        e.AssertToken(settingNameKind, settingNameText, settingNameValue);
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(settingValueKind, settingValueText, settingValue);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Theory]
    [InlineData(SyntaxKind.LessToken, "<")]
    [InlineData(SyntaxKind.GraterToken, ">")]
    [InlineData(SyntaxKind.LessGraterToken, "<>")]
    [InlineData(SyntaxKind.MinusToken, "-")]
    public void Parse_RelationshipColumnSettingClause_With_Left_Relation_Explicit(
        SyntaxKind relationshipTypeKind,
        string relationshipTypeText)
    {
        string fromSchemaName = CreateRandomString();
        string fromTableName = CreateRandomString();
        string fromColumnName = CreateRandomString();
        string fromIdentifierText = $"{fromSchemaName}.{fromTableName}.{fromColumnName}";
        string toSchemaName = CreateRandomString();
        string toTableName = CreateRandomString();
        string toColumnName = CreateRandomString();
        string toIdentifierText = $"{toSchemaName}.{toTableName}.{toColumnName}";
        string settingText = $"{fromIdentifierText} {relationshipTypeText} {toIdentifierText}";
        string text = $"{CreateRandomString()} {CreateRandomString()} [ ref: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.RelationshipColumnSettingClause);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.RelationshipConstraintClause);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, fromSchemaName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromTableName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, fromColumnName);
        e.AssertToken(relationshipTypeKind, relationshipTypeText);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, toSchemaName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toTableName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toColumnName);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Theory]
    [InlineData(SyntaxKind.LessToken, "<")]
    [InlineData(SyntaxKind.GraterToken, ">")]
    [InlineData(SyntaxKind.LessGraterToken, "<>")]
    [InlineData(SyntaxKind.MinusToken, "-")]
    public void Parse_RelationshipColumnSettingClause_With_Left_Relation_Implicit(
        SyntaxKind relationshipTypeKind,
        string relationshipTypeText)
    {
        string toSchemaName = CreateRandomString();
        string toTableName = CreateRandomString();
        string toColumnName = CreateRandomString();
        string toIdentifierText = $"{toSchemaName}.{toTableName}.{toColumnName}";
        string settingText = $"{relationshipTypeText} {toIdentifierText}";
        string text = $"{CreateRandomString()} {CreateRandomString()} [ ref: {settingText} ]";

        ColumnSettingListSyntax columnSettingListClause = ParseColumnSettingListClause(text);

        using AssertingEnumerator e = new AssertingEnumerator(columnSettingListClause);
        e.AssertNode(SyntaxKind.ColumnSettingListClause);
        e.AssertToken(SyntaxKind.OpenBracketToken, "[");
        e.AssertNode(SyntaxKind.RelationshipColumnSettingClause);
        e.AssertToken(SyntaxKind.RefKeyword, "ref");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertNode(SyntaxKind.RelationshipConstraintClause);
        e.AssertToken(relationshipTypeKind, relationshipTypeText);
        e.AssertNode(SyntaxKind.ColumnIdentifierClause);
        e.AssertToken(SyntaxKind.IdentifierToken, toSchemaName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toTableName);
        e.AssertToken(SyntaxKind.DotToken, ".");
        e.AssertToken(SyntaxKind.IdentifierToken, toColumnName);
        e.AssertToken(SyntaxKind.CloseBracketToken, "]");
    }

    [Theory]
    [InlineData("pk", "pk")]
    [InlineData("primarykey", "primary key")]
    [InlineData("null", "null")]
    [InlineData("notnull", "not null")]
    [InlineData("unique", "unique")]
    [InlineData("increment", "increment")]
    [InlineData("default", "default: Some_value")]
    [InlineData("default", "default: \"Some value\"")]
    [InlineData("default", "default: \'Some value\'")]
    [InlineData("note", "note: \"Some value\"")]
    [InlineData("note", "note: \'Some value\'")]
    public void Parse_ColumnSettingClause_With_Warning_Column_Setting_Already_Declared(
        string settingName, string settingText)
    {
        string text = $$"""
        {{CreateRandomString()}} {{CreateRandomString()}} [ {{settingText}}, {{settingText}} ]
        """;

        ImmutableArray<Diagnostic> diagnostics = ParseDiagnostics(text);
        Diagnostic diagnostic = Assert.Single(diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Column setting '{settingName}' already declared.", diagnostic.Message);
    }
}
