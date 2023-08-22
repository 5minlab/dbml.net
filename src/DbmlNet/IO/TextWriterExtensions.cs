using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.IO
{
    /// <summary>
    /// </summary>
    public static class TextWriterExtensions
    {
        private static bool IsConsole(this TextWriter writer)
        {
            if (writer == Console.Out)
                return !Console.IsOutputRedirected;

            else if (writer == Console.Error)
                // Color codes are always output to Console.Out
                return !Console.IsErrorRedirected && !Console.IsOutputRedirected;

            else if (writer is IndentedTextWriter iw && iw.InnerWriter.IsConsole())
                return true;

            return false;
        }

        private static void SetForeground(this TextWriter writer, ConsoleColor color)
        {
            if (writer.IsConsole())
                Console.ForegroundColor = color;
        }

        private static void ResetColor(this TextWriter writer)
        {
            if (writer.IsConsole())
                Console.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteInformation(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkCyan);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteSuccess(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkGreen);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteWarning(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkYellow);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteError(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkRed);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteDebug(this TextWriter writer, string message)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkGray);
            writer.Write(message);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="kind"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteKeyword(this TextWriter writer, SyntaxKind kind)
        {
            ArgumentNullException.ThrowIfNull(writer);

            Debug.Assert(kind.IsKeyword());
            string? text = kind.GetKnownText();
            Debug.Assert(text != null);

            writer.WriteKeyword(text);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteKeyword(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.Blue);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteIdentifier(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkYellow);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteNumber(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkCyan);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteString(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkMagenta);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public static void WriteSpace(this TextWriter writer)
        {
            writer.WritePunctuation(" ");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="kind"></param>
        public static void WritePunctuation(this TextWriter writer, SyntaxKind kind)
        {
            string? text = SyntaxFacts.GetKnownText(kind);
            Debug.Assert(text != null);

            writer.WritePunctuation(text);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WritePunctuation(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkGray);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WriteComment(this TextWriter writer, string text)
        {
            ArgumentNullException.ThrowIfNull(writer);

            writer.SetForeground(ConsoleColor.DarkGreen);
            writer.Write(text);
            writer.ResetColor();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="diagnostics"></param>
        public static void WriteDiagnostics(this TextWriter writer, ICollection<Diagnostic> diagnostics)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(diagnostics);

            foreach (Diagnostic diagnostic in diagnostics
                                                .Where(d => d.Location.Text == null))
            {
                if (diagnostic.IsWarning)
                    writer.WriteWarning(diagnostic.Message);
                else
                    writer.WriteError(diagnostic.Message);

                writer.WriteLine();
            }

            foreach (Diagnostic diagnostic in diagnostics
                                                .Where(d => d.Location.Text != null)
                                                .OrderBy(d => d.Location.FileName)
                                                .ThenBy(d => d.Location.Span.Start)
                                                .ThenBy(d => d.Location.Span.Length))
            {
                SourceText text = diagnostic.Location.Text;
                string fileName = diagnostic.Location.FileName;
                int startLine = diagnostic.Location.StartLine + 1;
                int startCharacter = diagnostic.Location.StartCharacter + 1;
                int endLine = diagnostic.Location.EndLine + 1;
                int endCharacter = diagnostic.Location.EndCharacter + 1;

                TextSpan span = diagnostic.Location.Span;
                int lineIndex = text.GetLineIndex(span.Start);
                TextLine line = text.Lines[lineIndex];

                writer.WriteLine();

                string reportedDiagnostic = $"{fileName}({startLine},{startCharacter},{endLine},{endCharacter}): {diagnostic}";
                if (diagnostic.IsWarning)
                    writer.WriteWarning(reportedDiagnostic);
                else
                    writer.WriteError(reportedDiagnostic);

                writer.WriteLine();
                writer.ResetColor();

                TextSpan prefixSpan = TextSpan.FromBounds(line.Start, span.Start);
                TextSpan suffixSpan = TextSpan.FromBounds(span.End, line.End);

                string prefix = text.ToString(prefixSpan);
                string error = text.ToString(span);
                string suffix = text.ToString(suffixSpan);

                writer.Write("    ");
                writer.Write(prefix);

                writer.SetForeground(ConsoleColor.DarkRed);
                writer.Write(error);
                writer.ResetColor();

                writer.Write(suffix);

                writer.WriteLine();
            }

            writer.WriteLine();
        }
    }
}

