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
}
