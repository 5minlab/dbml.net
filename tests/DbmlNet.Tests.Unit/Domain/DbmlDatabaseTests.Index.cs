using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Index_Empty()
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        Assert.Empty(table.Indexes);
    }

    [Fact]
    public void Create_Returns_Index_With_Identifier()
    {
        string randomIndexName = CreateRandomString();
        string indexText = $"{randomIndexName}";
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{indexText}}
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(indexText, index.Name);
        Assert.Equal(indexText, index.ToString());
        Assert.Equal(indexText, index.ColumnName);
        Assert.NotNull(index.Table);
        Assert.Equal(table, index.Table);
        Assert.False(index.IsPrimaryKey, "Column should not be primary key");
        Assert.False(index.IsUnique, "Column should not be unique");
        Assert.Null(index.Type);
        Assert.Null(index.Note);
        Assert.Empty(index.Settings);
    }
}
