using System;
using System.Linq;
using System.Text;

using DbmlNet.CodeAnalysis.Syntax;

using Tynamix.ObjectFiller;

internal static class DataGenerator
{
    /// <summary>
    /// Generates a random number between min and max.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>A random number between min and max.</returns>
    public static int GetRandomNumber(int min = int.MinValue, int max = int.MaxValue) =>
        new IntRange(min: min, max).GetValue();

    /// <summary>
    /// Generates a random decimal number between min and max.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>A random decimal number between min and max.</returns>
    public static decimal GetRandomDecimal(decimal min = decimal.MinValue, decimal max = decimal.MaxValue) =>
        new SequenceGeneratorDecimal { From = min, To = max }.GetValue();

    /// <summary>
    /// Generates a random string.
    /// </summary>
    /// <returns>A random string.</returns>
    public static string CreateRandomString() =>
        new MnemonicString().GetValue();

    /// <summary>
    /// Generates a random multi-word string.
    /// </summary>
    /// <returns>A random multi-word string.</returns>
    public static string CreateRandomMultiWordString() =>
        new MnemonicString(wordCount: GetRandomNumber(min: 1, max: 10)).GetValue();

    /// <summary>
    /// Generates a random multi-line text with random number of lines between min and max.
    /// </summary>
    /// <param name="minLineCount">The minimum number of lines.</param>
    /// <param name="maxLineCount">The maximum number of lines.</param>
    /// <returns>A random multi-line text.</returns>
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

    /// <summary>
    /// Gets the SQL Server data types.
    /// </summary>
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

    /// <summary>
    /// Gets a random keyword kind, text, and its value.
    /// </summary>
    /// <param name="keywordKind">The keyword kind.</param>
    /// <param name="keywordText">The keyword text.</param>
    /// <param name="keywordValue">The keyword value.</param>
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

    /// <summary>
    /// Gets a random syntax kind.
    /// </summary>
    /// <returns>A random syntax kind.</returns>
    /// <exception cref="Exception">In case a syntax kind cannot be generated.</exception>
    public static SyntaxKind GetRandomSyntaxKind()
    {
        int min = Enum.GetValues<SyntaxKind>().Min(kind => (int)kind);
        int max = Enum.GetValues<SyntaxKind>().Max(kind => (int)kind);
        int randomNumber = new IntRange(min, max).GetValue();

        return Enum.TryParse($"{randomNumber}", out SyntaxKind randomKind)
            ? randomKind
            : throw new Exception($"ERROR: Cannot generate random SyntaxKind from <{randomNumber}>.");
    }
}
