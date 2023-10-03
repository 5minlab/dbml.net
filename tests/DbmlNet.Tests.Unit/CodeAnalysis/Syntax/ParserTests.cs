using System.Collections.Immutable;
using System.Linq;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;

using Tynamix.ObjectFiller;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    private static ImmutableArray<Diagnostic> ParseDiagnostics(string text)
    {
        SyntaxTree syntaxTree = SyntaxTree.Parse(text);
        return syntaxTree.Diagnostics;
    }

    private static MemberSyntax ParseMember(string text)
    {
        SyntaxTree syntaxTree = SyntaxTree.Parse(text);
        Assert.Equal(SyntaxKind.CompilationUnitMember, syntaxTree.Root.Kind);
        Assert.True(syntaxTree.Root.Members.Any(), "Expected at least one syntax member, currently zero.");
        MemberSyntax member = Assert.Single(syntaxTree.Root.Members);
        Assert.NotNull(syntaxTree.Root.EndOfFileToken);
        return member;
    }

    private static StatementSyntax ParseStatement(string text)
    {
        MemberSyntax member = ParseMember(text);
        Assert.Equal(SyntaxKind.GlobalStatementMember, member.Kind);
        StatementSyntax statement =
            Assert.IsAssignableFrom<GlobalStatementSyntax>(member).Statement;
        return statement;
    }

    private static ExpressionSyntax ParseExpression(string text)
    {
        StatementSyntax statement = ParseStatement(text);
        ExpressionSyntax expression =
            Assert.IsAssignableFrom<ExpressionStatementSyntax>(statement).Expression;
        return expression;
    }

    private static ColumnSettingListSyntax ParseColumnSettingListClause(string text)
    {
        StatementSyntax statement = ParseStatement(text);
        ColumnDeclarationSyntax columnDeclarationStatement =
            Assert.IsAssignableFrom<ColumnDeclarationSyntax>(statement);
        Assert.NotNull(columnDeclarationStatement.SettingList);
        return columnDeclarationStatement.SettingList;
    }

    static readonly string[] SqlServerDataTypes = new string[]
    {
        // Exact numerics: These are data types that store integer or decimal numbers with exact precision and scale.
        "bigint",
        "bit",
        "decimal",
        "int",
        "money",
        "numeric",
        "smallint",
        "smallmoney",
        "tinyint",

        // Approximate numerics: These are data types that store floating-point numbers with approximate precision and scale.
        "float",
        "real",

        // Date and time: These are data types that store date and time values with various levels of accuracy and range.
        "date",
        "datetime2",
        "datetime",
        "datetimeoffset",
        "smalldatetime",
        "time",

        // Character strings: These are data types that store character data of fixed or variable length.
        "char",
        "char(1)",
        "char(8000)",

        "varchar",
        "varchar(1)",
        "varchar(8000)",
        "varchar(MAX)",

        "text",

        // Unicode character strings: These are data types that store Unicode character data of fixed or variable length.
        "ncar",
        "ncar(1)",
        "ncar(8000)",

        "nvarchar",
        "nvarchar(1)",
        "nvarchar(8000)",
        "nvarchar(MAX)",

        "ntext",
        "nenum",

        // Binary strings: These are data types that store binary data of fixed or variable length.
        "binary",
        "varbinary",
        "blob",
        "image",
    };

    private static int GetRandomNumber() =>
        new IntRange(min: 0, max: 10).GetValue();

    private static decimal GetRandomDecimal() =>
        new SequenceGeneratorDecimal { From = 0.0M, To = decimal.MaxValue }.GetValue();

    private static string CreateRandomString() =>
        new MnemonicString().GetValue();

    private static string CreateRandomMultiWordString() =>
        new MnemonicString(wordCount: GetRandomNumber()).GetValue();
}
