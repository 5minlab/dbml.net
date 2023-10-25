using System;
using System.Collections.Generic;
using System.IO;

namespace DbmlNet.CodeAnalysis.Text;

internal static class TextUnindenter
{
    public static string Unindent(string text)
    {
        string[] lines = UnindentLines(text);
        return string.Join(Environment.NewLine, lines);
    }

    public static string[] UnindentLines(string text)
    {
        List<string> lines = new();

        using (StringReader reader = new(text))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);
        }

        int minIndentation = int.MaxValue;
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];

            if (line.Trim().Length == 0)
            {
                lines[i] = string.Empty;
                continue;
            }

            int indentation = line.Length - line.TrimStart().Length;
            minIndentation = Math.Min(minIndentation, indentation);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Length == 0)
                continue;

            lines[i] = lines[i].Substring(minIndentation);
        }

        while (lines.Count > 0 && lines[0].Length == 0)
            lines.RemoveAt(0);

        while (lines.Count > 0 && lines[lines.Count - 1].Length == 0)
            lines.RemoveAt(lines.Count - 1);

        return lines.ToArray();
    }
}
