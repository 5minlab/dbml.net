using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed class DbmlDatabaseTests
{
    [Fact]
    public void Create_Database_Empty()
    {
        string text = "";
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.Null(database.Project);
        Assert.Empty(database.Tables);
        Assert.Empty(database.Providers);
        Assert.Empty(database.Note);
        Assert.Empty(database.Notes);
    }

    [Fact]
    public void Create_Database_With_Project_Name()
    {
        string text = """
        Project "AdventureWorks" {
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.NotNull(database.Project);
        Assert.Equal("AdventureWorks", database.Project.Name);
        Assert.Empty(database.Project.Notes);
        Assert.Empty(database.Tables);
        Assert.Empty(database.Providers);
        Assert.Empty(database.Note);
        Assert.Empty(database.Notes);
    }
}
