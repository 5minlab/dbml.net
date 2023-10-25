using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;

using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

#pragma warning disable CA1021 // Avoid out parameters

/// <summary>
/// Represents a syntax tree.
/// </summary>
public sealed class SyntaxTree
{
    private Dictionary<SyntaxNode, SyntaxNode?>? _parents;

    private SyntaxTree(SourceText text, ParseHandler handler)
    {
        Text = text;

        handler(
            this, out CompilationUnitSyntax root, out ImmutableArray<Diagnostic> diagnostics);

        Diagnostics = diagnostics;
        Root = root;
    }

    private delegate void ParseHandler(
        SyntaxTree syntaxTree,
        out CompilationUnitSyntax root,
        out ImmutableArray<Diagnostic> diagnostics);

    /// <summary>
    /// Gets the source text.
    /// </summary>
    public SourceText Text { get; }

    /// <summary>
    /// Gets the diagnostics.
    /// </summary>
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    /// <summary>
    /// Gets the root syntax node.
    /// </summary>
    public CompilationUnitSyntax Root { get; }

    /// <summary>
    /// Loads a syntax tree from the specified file.
    /// </summary>
    /// <param name="fileName">The name of the file to load.</param>
    /// <returns>The loaded syntax tree.</returns>
    public static SyntaxTree Load(string fileName)
    {
        string text = File.ReadAllText(fileName);
        SourceText sourceText = SourceText.From(text, fileName);
        return Parse(sourceText);
    }

    /// <summary>
    /// Parses the specified text and returns a syntax tree.
    /// </summary>
    /// <param name="text">The text to parse.</param>
    /// <returns>The syntax tree.</returns>
    public static SyntaxTree Parse(string text)
    {
        SourceText sourceText = SourceText.From(text);
        return Parse(sourceText);
    }

    /// <summary>
    /// Parses the specified text and returns a syntax tree.
    /// </summary>
    /// <param name="text">The text to parse.</param>
    /// <returns>The syntax tree.</returns>
    public static SyntaxTree Parse(SourceText text)
    {
        static void Parse(SyntaxTree syntaxTree, out CompilationUnitSyntax root, out ImmutableArray<Diagnostic> diagnostics)
        {
            Parser parser = new(syntaxTree);
            root = parser.ParseCompilationUnit();
            diagnostics = parser.Diagnostics.ToImmutableArray();
        }

        return new SyntaxTree(text, Parse);
    }

    /// <summary>
    /// Parses the given text and returns a collection of syntax tokens.
    /// </summary>
    /// <param name="text">The text to parse.</param>
    /// <param name="includeEndOfFile">A flag indicating whether to include the end of file token.</param>
    /// <returns>An immutable array of syntax tokens.</returns>
    public static ImmutableArray<SyntaxToken> ParseTokens(
        string text, bool includeEndOfFile = false)
    {
        SourceText sourceText = SourceText.From(text);
        return ParseTokens(sourceText, includeEndOfFile);
    }

    /// <summary>
    /// Parses the given text into an immutable array of syntax tokens.
    /// </summary>
    /// <param name="text">The text to parse.</param>
    /// <param name="diagnostics">The diagnostics produced during parsing.</param>
    /// <param name="includeEndOfFile">A flag indicating whether to include the end of file token.</param>
    /// <returns>An immutable array of syntax tokens.</returns>
    public static ImmutableArray<SyntaxToken> ParseTokens(
        string text, out ImmutableArray<Diagnostic> diagnostics, bool includeEndOfFile = false)
    {
        SourceText sourceText = SourceText.From(text);
        return ParseTokens(sourceText, out diagnostics, includeEndOfFile);
    }

    /// <summary>
    /// Parses the given source text into an immutable array of syntax tokens.
    /// </summary>
    /// <param name="text">The source text to parse.</param>
    /// <param name="includeEndOfFile">A flag indicating whether to include the end of file token.</param>
    /// <returns>An immutable array of syntax tokens.</returns>
    public static ImmutableArray<SyntaxToken> ParseTokens(
        SourceText text, bool includeEndOfFile = false)
    {
        return ParseTokens(text, out _, includeEndOfFile);
    }

    /// <summary>
    /// Parses the source text and returns an immutable array of syntax tokens.
    /// </summary>
    /// <param name="text">The source text to parse.</param>
    /// <param name="diagnostics">The diagnostics produced during the parse operation.</param>
    /// <param name="includeEndOfFile">A flag indicating whether to include the end of file token.</param>
    /// <returns>An immutable array of syntax tokens.</returns>
    public static ImmutableArray<SyntaxToken> ParseTokens(
        SourceText text, out ImmutableArray<Diagnostic> diagnostics, bool includeEndOfFile = false)
    {
        List<SyntaxToken> tokens = new();

        void ParseTokens(SyntaxTree st, out CompilationUnitSyntax root, out ImmutableArray<Diagnostic> d)
        {
            Lexer l = new(st);
            while (true)
            {
                SyntaxToken token = l.Lex();

                if (token.Kind != SyntaxKind.EndOfFileToken || includeEndOfFile)
                    tokens.Add(token);

                if (token.Kind == SyntaxKind.EndOfFileToken)
                {
                    root = new CompilationUnitSyntax(st, ImmutableArray<MemberSyntax>.Empty, token);
                    break;
                }
            }

            d = l.Diagnostics.ToImmutableArray();
        }

        SyntaxTree syntaxTree = new(text, ParseTokens);
        diagnostics = syntaxTree.Diagnostics;
        return tokens.ToImmutableArray();
    }

    internal SyntaxNode? GetParent(SyntaxNode syntaxNode)
    {
        if (_parents == null)
        {
            Dictionary<SyntaxNode, SyntaxNode?> parents = CreateParentsDictionary(Root);
            Interlocked.CompareExchange(ref _parents, parents, comparand: null);
        }

        return _parents[syntaxNode];
    }

    private Dictionary<SyntaxNode, SyntaxNode?> CreateParentsDictionary(CompilationUnitSyntax root)
    {
        Dictionary<SyntaxNode, SyntaxNode?> result = new()
        {
            { root, null }
        };

        CreateParentsDictionary(result, root);
        return result;
    }

    private void CreateParentsDictionary(Dictionary<SyntaxNode, SyntaxNode?> result, SyntaxNode node)
    {
        foreach (SyntaxNode child in node.GetChildren())
        {
            result.Add(child, node);
            CreateParentsDictionary(result, child);
        }
    }
}
