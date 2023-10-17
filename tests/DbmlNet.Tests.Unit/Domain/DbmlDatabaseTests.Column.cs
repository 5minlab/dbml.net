using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Database_Returns_Column_With_Name_And_Type()
    {
        string text = """
        Table Users
        {
            Id nvarchar(450)
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.Equal("Id", column.Name);
        Assert.Equal("nvarchar(450)", column.Type);
    }

    [Fact]
    public void Create_Database_Returns_Column_With_Empty_Settings()
    {
        string text = """
        Table Users
        {
            Id nvarchar(450) [ ]
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.NotNull(column.Table);
        Assert.Equal(table, column.Table);
        Assert.Equal(450, column.MaxLength);
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
}
