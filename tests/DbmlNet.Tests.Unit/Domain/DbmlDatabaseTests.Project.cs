using System.Linq;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
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
        Assert.Equal("AdventureWorks", database.Project.ToString());
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
        Assert.Equal("AdventureWorks", database.Project.ToString());
        string note = Assert.Single(database.Project.Notes);
        Assert.Equal("Contacts database schema.", note);
        Assert.Equal("Contacts database schema.", database.Project.Note);
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
        string provider = Assert.Single(database.Providers);
        Assert.Equal("PostgreSQL", provider);
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
        Assert.Single(database.Providers);
        Assert.Equal("PostgreSQL", database.Providers.ElementAt(0));
        Assert.NotNull(database.Project);
        Assert.Equal("AdventureWorks", database.Project.Name);
        Assert.Equal("AdventureWorks", database.Project.ToString());
        string note = Assert.Single(database.Project.Notes);
        Assert.Equal("AdventureWorksDW is the data warehouse sample", note);
        Assert.Equal("AdventureWorksDW is the data warehouse sample", database.Project.Note);
    }
}
