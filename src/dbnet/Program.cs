﻿using System;
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

string inputPath = args.FirstOrDefault() ?? string.Empty;
List<string> arguments = new List<string>(Environment.GetCommandLineArgs())
    .Skip(1).ToList();

bool ignoreWarnings = arguments.Any(arg => arg == "--ignore-warnings");
bool printSyntax = arguments.Any(arg => arg == "--print");
bool isOutputDirectorySpecified = arguments.Any(arg => new[] { "-o", "--output" }.Contains(arg));
bool printHelp = arguments.Any(arg => new[] { "-h", "--help" }.Contains(arg));
bool outputToMarkdown = true;
string? outputPath = null;
if (isOutputDirectorySpecified)
{
    int outputArgumentIndex =
        arguments.IndexOf("--output") < 0
        ? arguments.IndexOf("-o")
        : arguments.IndexOf("--output");

    bool hasOutputPathArg = arguments.Count > outputArgumentIndex + 1;
    if (hasOutputPathArg)
    {
        outputPath = arguments[outputArgumentIndex + 1];
    }

    if (string.IsNullOrEmpty(outputPath))
    {
        writer.WriteErrorMessage("Output path is not specified.");
        writer.WriteLine();
        PrintHelp();
        return;
    }
}
const string helpMessage = """
    Usage: dbnet <file-or-directory-path> [options]

    dbnet is a utility for converting *.dbml syntax to either *.sql or *.md format. Specify the desired output type using the --output-type option.

    Arguments:
      <file-or-directory-path>  File or directory to process.

    Options:
      --ignore-warnings         Process files with syntax tree warnings.
      --print-syntax            Display the syntax tree.
      --output-type <option>    Output type: [sql | markdown]
      -o --output <OUTPUT_DIR>  Set output directory for resulting files.
      -h --help                 Display command line help.

    Examples:
      dbnet file.dbml          # Convert a file
      dbnet ./dir-name/        # Convert all files in the folder


    """;

if (printHelp)
{
    PrintHelp();
    return;
}

if (!Path.Exists(inputPath))
{
    writer.WriteErrorMessage($"path does not exist or is not accessible '{inputPath}'.");
    writer.WriteLine();
    PrintHelp();
    return;
}

writer.WriteInfoMessage($"Lookup '*{ApplicationSettings.DbmlExtension}' files in input '{inputPath}'.");

string[] files = ApplicationSettings.FindDbmlNetFiles(inputPath);
if (files.Length == 0)
{
    writer.WriteErrorMessage($"No '*{ApplicationSettings.DbmlExtension}' files found in input '{inputPath}'.");
    return;
}

writer.WriteInfoMessage($"Found ({files.Length}) '*{ApplicationSettings.DbmlExtension}' files in input '{inputPath}'.");

int warningDiagnostics = 0;
int errorDiagnostics = 0;

Dictionary<string, SyntaxTree> fileSyntaxTreeList = new();

foreach (string filePath in files)
{
    writer.WriteInfoMessage($"Start parsing file '{filePath}'.");

    string inputText = File.ReadAllText(filePath);
    SyntaxTree syntaxTree = SyntaxTree.Parse(inputText);

    if (printSyntax)
    {
        PrintSyntax(writer, filePath, syntaxTree);
    }

    errorDiagnostics += syntaxTree.Diagnostics.Count(s => s.IsError);
    warningDiagnostics += syntaxTree.Diagnostics.Count(s => s.IsWarning);

    if (syntaxTree.Diagnostics.Length > 0)
    {
        writer.WriteDiagnostics(syntaxTree.Diagnostics);
        bool allWarnings = syntaxTree.Diagnostics.All(s => s.IsWarning);
        bool skipFile = allWarnings && !ignoreWarnings;
        if (skipFile)
        {
            string message = $"""
            Skipping file '{filePath}' due to warnings. Use '--ignore-warnings' to allow files to be processed.
            """;
            writer.WriteWarningMessage(message);
            continue; // skipping file due to diagnostics
        }
    }

    fileSyntaxTreeList.Add(filePath, syntaxTree);
}

if (outputToMarkdown)
{
    foreach (KeyValuePair<string, SyntaxTree> fileSyntaxTree in fileSyntaxTreeList)
    {
        string dbmlFilePath = fileSyntaxTree.Key;
        SyntaxTree dbmlSyntaxTree = fileSyntaxTree.Value;
        string outputDirectoryPath = Path.GetDirectoryName(dbmlFilePath) ?? "./";
        string outputFileName = Path.GetFileNameWithoutExtension(dbmlFilePath) + ".md";
        string outputFilePath = Path.Combine(outputPath ?? outputDirectoryPath, outputFileName);
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
writer.WriteLine($"{files.Length} found | {fileSyntaxTreeList.Count} processed | {files.Length - fileSyntaxTreeList.Count} skipped File(s).");
writer.WriteLine($"{warningDiagnostics} Warning(s)");
writer.WriteLine($"{errorDiagnostics} Error(s)");
writer.Indent -= 1;
writer.WriteLine();
writer.WriteLine($"Time Elapsed {buildWatch.Elapsed:hh':'mm':'ss'.'ff}");

void PrintHelp()
{
    writer.Write(helpMessage);
}

static void PrintSyntax(TextWriter writer, string filePath, SyntaxTree syntaxTree)
{
    string dbmlFileName = Path.GetFileName(filePath);
    writer.WriteLine();
    writer.WriteLine($"'{dbmlFileName}' syntax tree:");
    syntaxTree.Root.WriteTo(writer);
    writer.WriteLine();
}

