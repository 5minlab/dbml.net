using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Database_With_Table_Name()
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
    public void Create_Database_With_Table_Name_And_Schema()
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
    public void Create_Database_With_Table_Name_And_Schema_And_Database()
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
}
