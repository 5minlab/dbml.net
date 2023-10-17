using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Table_Empty()
    {
        string text = """
        Table Users
        {
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Empty(table.Database);
        Assert.Empty(table.Schema);
        Assert.Equal("Users", table.Name);
        Assert.Equal("Users", table.ToString());
        Assert.Empty(table.Columns);
        Assert.Empty(table.Indexes);
        Assert.Empty(table.Relationships);
        Assert.Empty(table.Notes);
        Assert.Empty(table.Note);
    }

    [Fact]
    public void Create_Returns_Table_With_Name()
    {
        string text = """
        Table Users
        {
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Equal("Users", table.Name);
        Assert.Equal("Users", table.ToString());
    }

    [Fact]
    public void Create_Returns_Table_With_Name_And_Schema()
    {
        string text = """
        Table identity.Users
        {
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Empty(table.Database);
        Assert.Equal("identity", table.Schema);
        Assert.Equal("Users", table.Name);
        Assert.Equal("identity.Users", table.ToString());
    }

    [Fact]
    public void Create_Returns_Table_With_Name_And_Schema_And_Database()
    {
        string text = """
        Table AdventureWorks.identity.Users
        {
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Equal("AdventureWorks", table.Database);
        Assert.Equal("identity", table.Schema);
        Assert.Equal("Users", table.Name);
        Assert.Equal("AdventureWorks.identity.Users", table.ToString());
    }

    [Fact]
    public void Create_Returns_Table_With_Note()
    {
        string text = """
        Table Users
        {
            note: 'This is a note.'
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        string note = Assert.Single(table.Notes);
        Assert.Equal("This is a note.", note);
        Assert.Equal("This is a note.", table.Note);
    }
}
