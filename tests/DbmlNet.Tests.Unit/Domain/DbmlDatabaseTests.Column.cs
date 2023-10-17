using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Column_Empty()
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.NotNull(column.Table);
        Assert.Equal(table, column.Table);
        Assert.Null(column.MaxLength);
        Assert.False(column.HasMaxLength, "Column should not have max length");
        Assert.False(column.IsPrimaryKey, "Column should not be primary key");
        Assert.False(column.IsUnique, "Column should not be unique");
        Assert.False(column.IsAutoIncrement, "Column should not be auto increment");
        Assert.False(column.IsNullable, "Column should not be nullable");
        Assert.True(column.IsRequired, "Column should be required");
        Assert.False(column.HasDefaultValue, "Column should not have default value");
        Assert.Null(column.DefaultValue);
        Assert.Empty(column.UnknownSettings);
        Assert.Null(column.Note);
        Assert.Empty(column.Notes);
    }

    [Fact]
    public void Create_Returns_Column_With_Name_And_Type()
    {
        string randomColumnName = CreateRandomString();
        string randomColumnType = CreateRandomString();
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{randomColumnName}} {{randomColumnType}}
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.Equal(randomColumnName, column.Name);
        Assert.Equal(randomColumnName, column.ToString());
        Assert.Equal(randomColumnType, column.Type);
    }

    [Theory]
    [InlineData("binary(MAX)", 1.7976931348623157E+308)]
    [InlineData("varbinary(MAX)", 1.7976931348623157E+308)]
    [InlineData("char(MAX)", 1.7976931348623157E+308)]
    [InlineData("varchar(MAX)", 1.7976931348623157E+308)]
    [InlineData("nchar(MAX)", 1.7976931348623157E+308)]
    [InlineData("nvarchar(MAX)", 1.7976931348623157E+308)]
    public void Create_Returns_Column_With_Max_Length(string columnTypeText, object? maxLength)
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{columnTypeText}}
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.HasMaxLength, "Column should have max length");
        Assert.NotNull(column.MaxLength);
        Assert.Equal(maxLength, column.MaxLength);
    }

    [Fact]
    public void Create_Returns_Column_With_Note()
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ note: 'This is a note.' ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        string note = Assert.Single(column.Notes);
        Assert.Equal("This is a note.", note);
        Assert.Equal("This is a note.", column.Note);
    }

    [Theory]
    [InlineData("pk")]
    [InlineData("primary key")]
    public void Create_Returns_Column_With_PrimaryKey_Flag(string primaryKeyText)
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ {{primaryKeyText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsPrimaryKey, "Column should be primary key");
    }

    [Fact]
    public void Create_Returns_Column_With_Unique_Flag()
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ unique ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsUnique, "Column should be unique");
    }

    [Fact]
    public void Create_Returns_Column_With_Increment_Flag()
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ increment ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsAutoIncrement, "Column should be auto increment");
    }

    [Fact]
    public void Create_Returns_Column_With_Nullable_Flag()
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ null ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsNullable, "Column should be nullable");
        Assert.False(column.IsRequired, "Column should not be required");
    }

    [Fact]
    public void Create_Returns_Column_With_Not_Nullable_Flag()
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ not null ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.False(column.IsNullable, "Column should not be nullable");
        Assert.True(column.IsRequired, "Column should be required");
    }

    [Theory]
    [InlineData("someDefaultValue", "someDefaultValue")]
    [InlineData("123", "123")]
    [InlineData("123.456", "123.456")]
    [InlineData("true", "true")]
    [InlineData("false", "false")]
    [InlineData("null", null)]
    [InlineData("'some string value'", "some string value")]
    [InlineData("\"some string value\"", "some string value")]
    [InlineData("`now()`", null)]
    public void Create_Returns_Column_With_Default_Value(string valueText, object? value)
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ default: {{valueText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        if (value is null)
            Assert.False(column.HasDefaultValue, "Column should not have default value");
        else
            Assert.True(column.HasDefaultValue, "Column should have default value");
        Assert.Equal(value, column.DefaultValue);
    }

    [Fact]
    public void Create_Returns_Column_With_Unknown_Setting()
    {
        string settingName = CreateRandomString();
        object? settingValue = null;
        string settingText = $"{settingName}";
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ {{settingText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown column setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(column.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }

    [Fact]
    public void Create_Returns_Column_With_Unknown_Setting_Identifier_Value()
    {
        string settingName = CreateRandomString();
        object? settingValue = CreateRandomString();
        string settingText = $"{settingName}: {settingValue}";
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ {{settingText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown column setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(column.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }

    [Fact]
    public void Create_Returns_Column_With_Unknown_Setting_QuotationMarksString_Value()
    {
        string settingName = CreateRandomString();
        object? settingValue = CreateRandomMultiWordString();
        string settingText = $"{settingName}: \"{settingValue}\"";
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            {{CreateRandomString()}} {{CreateRandomString()}} [ {{settingText}} ]
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown column setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(column.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }
}
