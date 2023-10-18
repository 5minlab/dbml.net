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

string inputPath = args.FirstOrDefault() ?? string.Empty;
string[] arguments = new List<string>(Environment.GetCommandLineArgs())
    .Skip(1).ToArray();

bool ignoreWarnings = arguments.Any(arg => arg == "--ignore-warnings");
bool printSyntax = arguments.Any(arg => arg == "--print");
bool printHelp = arguments.Any(arg => new[] { "-h", "--help" }.Contains(arg));
bool outputToMarkdown = true;
const string helpMessage = """
    Usage: dbnet [<file | directory>...] [options]

    dbnet helps converting *.dbml syntax to *.sql, *.md syntax. Check the --output-type option.

    Arguments:
      <file-or-directory-path> The file or directory path to operate on.

    Options:
      --ignore-warnings    Allow files be processed even if the syntax tree contains warnings.
      --print-syntax       Prints the syntax tree.
      --output-type <opt>  Output type to use. Supported values: [sql | markdown]
      -h --help            Show command line help.
    """;

if (printHelp)
{
    PrintHelp();
    return;
}

if (string.IsNullOrWhiteSpace(inputPath))
{
    writer.WriteErrorMessage("provide a valid file or directory path");
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

    if (syntaxTree.Diagnostics.Length > 0)
    {
        writer.WriteDiagnostics(syntaxTree.Diagnostics);
        bool allWarnings = syntaxTree.Diagnostics.All(s => s.IsWarning);
        bool skipFile = allWarnings && !ignoreWarnings;
        if (skipFile)
        {
            writer.WriteWarningMessage($"Skipping file '{filePath}' due to warnings.");
            writer.WriteInfoMessage($"Use '--ignore-warnings' to ignore warnings.");
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
writer.WriteLine($"{files.Length} found | {fileSyntaxTreeList.Count} processed | {files.Length - fileSyntaxTreeList.Count} skipped File(s).");
writer.Indent -= 1;
writer.WriteLine();
writer.WriteLine($"Time Elapsed {buildWatch.Elapsed:hh':'mm':'ss'.'ff}");

void PrintHelp()
{
    writer.Write(helpMessage);
    writer.WriteLine();
    writer.WriteLine();
}

static void PrintSyntax(TextWriter writer, string filePath, SyntaxTree syntaxTree)
{
    string dbmlFileName = Path.GetFileNameWithoutExtension(filePath);
    writer.WriteLine();
    writer.WriteLine($"'{dbmlFileName}' syntax tree:");
    syntaxTree.Root.WriteTo(writer);
    writer.WriteLine();
}

