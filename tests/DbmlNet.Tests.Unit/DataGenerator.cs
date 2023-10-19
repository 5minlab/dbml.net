using System;
using System.Linq;
using System.Text;

using DbmlNet.CodeAnalysis.Syntax;

using Tynamix.ObjectFiller;

internal static class DataGenerator
{
    public static int GetRandomNumber() =>
        new IntRange(min: 0, max: 10).GetValue();

    public static decimal GetRandomDecimal() =>
        new SequenceGeneratorDecimal { From = 0.0M, To = decimal.MaxValue }.GetValue();

    public static string CreateRandomString() =>
        new MnemonicString().GetValue();

    public static string CreateRandomMultiWordString() =>
        new MnemonicString(wordCount: new IntRange(min: 1, max: 10).GetValue()).GetValue();

    public static string CreateRandomMultiLineText(
        int minLineCount = int.MinValue,
        int maxLineCount = int.MaxValue)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = minLineCount; i < maxLineCount; i++)
        {
            sb.AppendLine(CreateRandomMultiWordString());
        }
        return sb.ToString();
    }

    public static readonly string[] SqlServerDataTypes = new string[]
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

    public static void GetRandomKeyword(
        out SyntaxKind keywordKind,
        out string keywordText,
        out object? keywordValue)
    {
        SyntaxKind[] keywordKinds =
            Enum.GetValues<SyntaxKind>()
                .Where(kind => kind.IsKeyword())
                .ToArray();

        int maxIndex = keywordKinds.Length == 0 ? 0 : keywordKinds.Length - 1;
        int randomIndex = new IntRange(min: 0, max: maxIndex).GetValue();
        keywordKind = keywordKinds[randomIndex];
        keywordText = SyntaxFacts.GetKnownText(keywordKind) ?? string.Empty;
        keywordValue = SyntaxFacts.GetKnownValue(keywordKind);
    }
}
