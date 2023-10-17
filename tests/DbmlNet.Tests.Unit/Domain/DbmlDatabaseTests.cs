using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;

using Tynamix.ObjectFiller;

using Xunit;

namespace DbmlNet.Tests.Unit.Domain;

public sealed partial class DbmlDatabaseTests
{
    [Fact]
    public void Create_Returns_Database_Empty()
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
    public void Create_Returns_Database_With_Note()
    {
        string randomNote = CreateRandomMultiWordString();
        string text = $$"""
        note: '{{randomNote}}'
        """;
        SyntaxTree syntax = SyntaxTree.Parse(text);

        DbmlDatabase database = DbmlDatabase.Create(syntax);

        Assert.NotNull(database);
        Assert.Equal(randomNote, database.Note);
    }

    private static SyntaxTree ParseNoDiagnostics(string text)
    {
        SyntaxTree syntax = SyntaxTree.Parse(text);
        Assert.Empty(syntax.Diagnostics);
        return syntax;
    }

    private static int GetRandomNumber() =>
        new IntRange(min: 0, max: 10).GetValue();

    private static decimal GetRandomDecimal() =>
        new SequenceGeneratorDecimal { From = 0.0M, To = decimal.MaxValue }.GetValue();

    private static string CreateRandomString() =>
        new MnemonicString().GetValue();

    private static string CreateRandomMultiWordString() =>
        new MnemonicString(wordCount: new IntRange(min: 1, max: 10).GetValue()).GetValue();
}
