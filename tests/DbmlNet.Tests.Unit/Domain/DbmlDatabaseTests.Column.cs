using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Column_Empty()
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

    [Fact]
    public void Create_Returns_Column_With_Name_And_Type()
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
        Assert.Equal("Id", column.ToString());
        Assert.Equal("nvarchar(450)", column.Type);
    }

    [Fact]
    public void Create_Returns_Column_With_Note()
    {
        string text = """
        Table Users
        {
            Id nvarchar(450) [ note: 'This is a note.' ]
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

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
        Table Users
        {
            Id nvarchar(450) [ {{primaryKeyText}} ]
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsPrimaryKey, "Column should be primary key");
    }

    [Fact]
    public void Create_Returns_Column_With_Unique_Flag()
    {
        string text = """
        Table Users
        {
            Id nvarchar(450) [ unique ]
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsUnique, "Column should be unique");
    }

    [Fact]
    public void Create_Returns_Column_With_Increment_Flag()
    {
        string text = """
        Table Users
        {
            Id nvarchar(450) [ increment ]
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.True(column.IsAutoIncrement, "Column should be auto increment");
    }

    [Fact]
    public void Create_Returns_Column_With_Nullable_Flag()
    {
        string text = """
        Table Users
        {
            Id nvarchar(450) [ null ]
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

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
        string text = """
        Table Users
        {
            Id nvarchar(450) [ not null ]
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableColumn column = Assert.Single(table.Columns);
        Assert.False(column.IsNullable, "Column should not be nullable");
        Assert.True(column.IsRequired, "Column should be required");
    }
}
