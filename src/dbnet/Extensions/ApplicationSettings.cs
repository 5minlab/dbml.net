using System;
using System.IO;

namespace DbmlNet.Extensions;

internal static class ApplicationSettings
{
    /// <summary>
    /// Returns the extension of dbml file (including the period ".").
    /// </summary>
    public const string DbmlExtension = ".dbml";

    public static string[] FindDbmlNetFiles(string inputPath)
    {
        if (Directory.Exists(inputPath))
            return Directory.GetFiles(inputPath, $"*{DbmlExtension}");

        return Path.GetExtension(inputPath).Equals(DbmlExtension, StringComparison.Ordinal)
            ? new[] { inputPath }
            : Array.Empty<string>();
    }
}
