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
List<string> arguments = new List<string>(Environment.GetCommandLineArgs())
    .Skip(1).ToList();

bool isForceEnabled = arguments.Exists(arg => arg.Equals("--force", StringComparison.Ordinal));
bool printSyntax = arguments.Exists(arg => arg.Equals("--print", StringComparison.Ordinal));
bool isOutputDirectorySpecified = arguments.Exists(arg => new[] { "-o", "--output" }.Contains(arg, StringComparer.InvariantCulture));
bool printHelp = arguments.Exists(arg => new[] { "-h", "--help" }.Contains(arg, StringComparer.InvariantCulture));
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
      --print-syntax            Display the syntax tree.
      --output-type <option>    Output type: [sql | markdown]
      --force                   Generate content, even if it modifies existing files.
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

Dictionary<string, SyntaxTree> fileSyntaxTreeList = new(StringComparer.InvariantCulture);

foreach (string filePath in files)
{
    writer.WriteInfoMessage($"Start parsing file '{filePath}'.");

    SyntaxTree syntaxTree = SyntaxTree.Load(filePath);

    if (printSyntax)
    {
        PrintSyntax(writer, syntaxTree);
    }

    errorDiagnostics += syntaxTree.Diagnostics.Count(s => s.IsError);
    warningDiagnostics += syntaxTree.Diagnostics.Count(s => s.IsWarning);

    if (syntaxTree.Diagnostics.Length > 0)
    {
        writer.WriteDiagnostics(syntaxTree.Diagnostics);
        bool allWarnings = syntaxTree.Diagnostics.All(s => s.IsWarning);
        if (!allWarnings)
        {
            string message = $"""
            Skipping file '{filePath}' due to errors.
            """;
            writer.WriteErrorMessage(message);
            continue; // skipping file due to diagnostics
        }

        bool skipFile = allWarnings && !isForceEnabled;
        if (skipFile)
        {
            string message = $"""
            Skipping file '{filePath}' due to warnings. Use '--force' to allow files to be processed.
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
if (fileSyntaxTreeList.Count > 0)
{
    writer.WriteSuccess("dbnet succeeded.");
    writer.WriteLine();
}

writer.Indent++;

writer.Write($"{files.Length} found");
writer.Write(" | ");

if (fileSyntaxTreeList.Count > 0)
    writer.WriteSuccess($"{fileSyntaxTreeList.Count} processed");
else
    writer.WriteError($"{fileSyntaxTreeList.Count} processed");

writer.Write(" | ");

if (files.Length - fileSyntaxTreeList.Count > 0)
    writer.WriteWarning($"{files.Length - fileSyntaxTreeList.Count} skipped");
else
    writer.Write("0 skipped");
writer.Write(" File(s).");

writer.WriteLine();

WriteWarningCount(writer, isForceEnabled, warningDiagnostics);
WriteErrorCount(writer, errorDiagnostics);
writer.Indent--;
writer.WriteLine();
writer.WriteLine($"Time Elapsed {buildWatch.Elapsed:hh':'mm':'ss'.'ff}");

void PrintHelp()
{
    writer.Write(helpMessage);
}

static void PrintSyntax(TextWriter writer, SyntaxTree syntaxTree)
{
    string dbmlFileName = syntaxTree.Text.FileName;
    writer.WriteLine();
    writer.WriteLine($"'{dbmlFileName}' syntax tree:");
    syntaxTree.Root.WriteTo(writer);
    writer.WriteLine();
}

static void WriteWarningCount(IndentedTextWriter writer, bool isForceEnabled, int warningDiagnostics)
{
    if (isForceEnabled)
        writer.Write($"{warningDiagnostics} Warning(s)");
    else if (warningDiagnostics > 0)
        writer.WriteWarning($"{warningDiagnostics} Warning(s)");
    else
        writer.Write($"{warningDiagnostics} Warning(s)");
    writer.WriteLine();
}

static void WriteErrorCount(IndentedTextWriter writer, int errorDiagnostics)
{
    if (errorDiagnostics > 0)
        writer.WriteError($"{errorDiagnostics} Error(s)");
    else
        writer.Write($"{errorDiagnostics} Error(s)");
    writer.WriteLine();
}
