using DbmlNet.CodeAnalysis;
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
    public void Create_Returns_Index_SingleField()
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
        Assert.Empty(index.UnknownSettings);
    }

    [Theory]
    [InlineData("pk")]
    [InlineData("primary key")]
    public void Create_Returns_Index_With_PrimaryKey_Flag(string settingText)
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ {{settingText}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.True(index.IsPrimaryKey, "Column should be primary key");
    }

    [Fact]
    public void Create_Returns_Index_With_Unique_Flag()
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ unique ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.True(index.IsUnique, "Column should be unique");
    }

    [Fact]
    public void Create_Returns_Index_With_QuotationMarksString_Name()
    {
        string columnName = CreateRandomString();
        string indexName = CreateRandomMultiWordString();
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{columnName}} [ name: "{{indexName}}" ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(indexName, index.Name);
        Assert.Equal(columnName, index.ColumnName);
    }

    [Fact]
    public void Create_Returns_Index_With_SingleQuotationMarksString_Name()
    {
        string columnName = CreateRandomString();
        string indexName = CreateRandomMultiWordString();
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{columnName}} [ name: '{{indexName}}' ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(indexName, index.Name);
        Assert.Equal(columnName, index.ColumnName);
    }

    [Theory]
    [InlineData("btree")]
    [InlineData("gin")]
    [InlineData("gist")]
    [InlineData("hash")]
    public void Create_Returns_Index_With_Allowed_Type(string typeName)
    {
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ type: {{typeName}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(typeName, index.Type);
    }

    [Fact]
    public void Create_Returns_Index_With_Unknown_Type_Identifier()
    {
        string typeName = CreateRandomString();
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ type: {{typeName}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(typeName, index.Type);
    }

    [Fact]
    public void Create_Returns_Index_With_Unknown_QuotationMarksString_Type()
    {
        string typeName = CreateRandomMultiWordString();
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ type: "{{typeName}}" ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(typeName, index.Type);
    }

    [Fact]
    public void Create_Returns_Index_With_Unknown_SingleQuotationMarksString_Type()
    {
        string typeName = CreateRandomMultiWordString();
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ type: '{{typeName}}' ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(typeName, index.Type);
    }

    [Fact]
    public void Create_Returns_Index_With_QuotationMarksString_Note()
    {
        string noteValue = CreateRandomMultiWordString();
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ note: "{{noteValue}}" ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(noteValue, index.Note);
    }

    [Fact]
    public void Create_Returns_Index_With_SingleQuotationMarksString_Note()
    {
        string noteValue = CreateRandomMultiWordString();
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ note: '{{noteValue}}' ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        Assert.Equal(noteValue, index.Note);
    }

    [Fact]
    public void Create_Returns_Index_With_Unknown_Setting()
    {
        string settingName = CreateRandomString();
        object? settingValue = null;
        string settingText = $"{settingName}";
        string text = $$"""
        Table {{CreateRandomString()}}
        {
            Indexes {
                {{CreateRandomString()}} [ {{settingText}} ]
            }
        }
        """;
        SyntaxTree syntax = ParseNoErrorDiagnostics(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Diagnostic diagnostic = Assert.Single(syntax.Diagnostics);
        Assert.False(diagnostic.IsError, "Should not be error");
        Assert.True(diagnostic.IsWarning, "Should be warning");
        Assert.Equal($"Unknown index setting '{settingName}'.", diagnostic.Message);
        Assert.NotNull(database);
        DbmlTable table = Assert.Single(database.Tables);
        DbmlTableIndex index = Assert.Single(table.Indexes);
        (string unknownSettingName, object? unknownSettingValue) = Assert.Single(index.UnknownSettings);
        Assert.Equal(settingName, unknownSettingName);
        Assert.Equal(settingValue, unknownSettingValue);
    }
}
