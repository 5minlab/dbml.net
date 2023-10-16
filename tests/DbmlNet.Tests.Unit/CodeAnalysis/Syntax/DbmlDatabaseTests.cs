using System.Linq;

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

    [Fact]
    public void Create_Database_With_Project_Note()
    {
        string text = """
        Project "AdventureWorks" {
            note: 'Contacts database schema.'
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.NotNull(database.Project);
        Assert.Equal("AdventureWorks", database.Project.Name);
        Assert.Equal(1, database.Project.Notes.Count());
        Assert.Equal("Contacts database schema.", database.Project.Note);
        Assert.Empty(database.Tables);
        Assert.Empty(database.Providers);
        Assert.Empty(database.Note);
        Assert.Empty(database.Notes);
    }

    [Fact]
    public void Create_Database_With_Project_DatabaseType()
    {
        string text = """
        Project "AdventureWorks" {
            database_type: 'PostgreSQL'
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.NotNull(database.Project);
        Assert.Equal("AdventureWorks", database.Project.Name);
        Assert.Empty(database.Project.Notes);
        Assert.Empty(database.Tables);
        Assert.Equal(1, database.Providers.Count());
        Assert.Equal("PostgreSQL", database.Providers.ElementAt(0));
        Assert.Empty(database.Note);
        Assert.Empty(database.Notes);
    }

    [Fact]
    public void Create_Database_With_Project_Fully_Setup()
    {
        string text = """
        Project "AdventureWorks" {
            database_type: 'PostgreSQL'
            note: 'AdventureWorksDW is the data warehouse sample'
        }
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.NotNull(database.Project);
        Assert.Equal("AdventureWorks", database.Project.Name);
        Assert.Equal(1, database.Project.Notes.Count());
        Assert.Equal("AdventureWorksDW is the data warehouse sample", database.Project.Note);
        Assert.Empty(database.Tables);
        Assert.Equal(1, database.Providers.Count());
        Assert.Equal("PostgreSQL", database.Providers.ElementAt(0));
        Assert.Empty(database.Note);
        Assert.Empty(database.Notes);
    }
}
