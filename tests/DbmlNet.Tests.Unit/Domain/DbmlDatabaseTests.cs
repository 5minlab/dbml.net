using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Database_Empty()
    {
        string text = "";
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.Empty(database.Providers);
        Assert.Empty(database.Notes);
        Assert.Empty(database.Note);
        Assert.Null(database.Project);
        Assert.Empty(database.Tables);
    }

    [Fact]
    public void Create_Database_With_Note()
    {
        string text = """
        note: 'This is a note.'
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.Equal("This is a note.", database.Note);
    }
}
