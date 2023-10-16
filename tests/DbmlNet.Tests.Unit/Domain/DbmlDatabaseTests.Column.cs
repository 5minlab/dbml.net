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
}
