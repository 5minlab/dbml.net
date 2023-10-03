using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;

using DbmlNet.CodeAnalysis;
using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.Domain;
using DbmlNet.Extensions;
using DbmlNet.IO;

Stopwatch buildWatch = new Stopwatch();
buildWatch.Start();

using IndentedTextWriter writer =
    new IndentedTextWriter(Console.Out);

bool outputSyntax = true;
bool outputToMarkdown = true;

if (args.Length <= 0)
{
    writer.WriteErrorMessage("provide a valid file or directory path");
    writer.WriteInfoMessage("usage: dbnet <file-or-directory-path>");
    return;
}

if (args.Length > 1)
{
    writer.WriteErrorMessage("only one path supported right now");
    return;
}

string inputPath = args.Single();

writer.WriteInfoMessage($"Lookup '*{ApplicationSettings.DbmlExtension}' files from input '{inputPath}'.");

string[] files = ApplicationSettings.FindDbmlNetFiles(inputPath);
if (files.Length == 0)
{
    writer.WriteErrorMessage($"No '*{ApplicationSettings.DbmlExtension}' files found in input '{inputPath}'.");
    return;
}

writer.WriteInfoMessage($"Found ({files.Length}) '*{ApplicationSettings.DbmlExtension}' files in input '{inputPath}'.");

foreach (string filePath in files)
{
    writer.WriteInfoMessage($"Start parsing file '{filePath}'.");

    string inputText = File.ReadAllText(filePath);
    SyntaxTree syntaxTree = SyntaxTree.Parse(inputText);

    if (syntaxTree.Diagnostics.Length > 0)
    {
        writer.WriteDiagnostics(syntaxTree.Diagnostics);
        return;
    }

    if (outputSyntax)
    {
        writer.WriteLine();
        writer.WriteLine("Syntax tree:");
        syntaxTree.PrintSyntaxTo(writer);
        writer.WriteLine();
    }

    if (outputToMarkdown)
    {
        string outputDirectoryPath = Path.GetDirectoryName(filePath) ?? "./";
        string fileName = Path.GetFileNameWithoutExtension(filePath) + ".md";
        string outputFilePath = Path.Combine(outputDirectoryPath, fileName);
        writer.WriteInfoMessage($"Writing to output '{outputFilePath}'.");

        DbmlDatabase dbmlDatabase = DbmlDatabase.Create(syntaxTree);
        DbmlMarkdownWriter.WriterToFile(dbmlDatabase, outputFilePath);
    }
}

buildWatch.Stop();

writer.WriteLine();
writer.WriteSuccess($"dbnet succeeded.");
writer.WriteLine();
writer.Indent += 1;
writer.WriteLine($"{files.Length} File(s).");
writer.Indent -= 1;
writer.WriteLine();
writer.WriteLine($"Time Elapsed {buildWatch.Elapsed}");
