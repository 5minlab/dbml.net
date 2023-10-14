using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

writer.WriteInfoMessage($"Lookup '*{ApplicationSettings.DbmlExtension}' files in input '{inputPath}'.");

string[] files = ApplicationSettings.FindDbmlNetFiles(inputPath);
if (files.Length == 0)
{
    writer.WriteErrorMessage($"No '*{ApplicationSettings.DbmlExtension}' files found in input '{inputPath}'.");
    return;
}

writer.WriteInfoMessage($"Found ({files.Length}) '*{ApplicationSettings.DbmlExtension}' files in input '{inputPath}'.");

Dictionary<string, SyntaxTree> fileSyntaxTreeList = new();

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

    fileSyntaxTreeList.Add(filePath, syntaxTree);
}

if (outputSyntax)
{
    foreach (KeyValuePair<string, SyntaxTree> fileSyntaxTree in fileSyntaxTreeList)
    {
        string dbmlFilePath = fileSyntaxTree.Key;
        string dbmlFileName = Path.GetFileNameWithoutExtension(dbmlFilePath);
        SyntaxTree dbmlSyntaxTree = fileSyntaxTree.Value;

        writer.WriteLine();
        writer.WriteLine($"Syntax tree of '{dbmlFileName}':");
        dbmlSyntaxTree.PrintSyntaxTo(writer);
    }

    writer.WriteLine();
}

if (outputToMarkdown)
{
    foreach (KeyValuePair<string, SyntaxTree> fileSyntaxTree in fileSyntaxTreeList)
    {
        string dbmlFilePath = fileSyntaxTree.Key;
        SyntaxTree dbmlSyntaxTree = fileSyntaxTree.Value;
        string outputDirectoryPath = Path.GetDirectoryName(dbmlFilePath) ?? "./";
        string outputFileName = Path.GetFileNameWithoutExtension(dbmlFilePath) + ".md";
        string outputFilePath = Path.Combine(outputDirectoryPath, outputFileName);
        writer.WriteInfoMessage($"Writing to output '{outputFilePath}'.");

        DbmlDatabase dbmlDatabase = DbmlDatabase.Create(dbmlSyntaxTree);
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
