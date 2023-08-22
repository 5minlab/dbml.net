using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;

using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

#pragma warning disable CA1021 // Avoid out parameters

/// <summary>
/// </summary>
public sealed class SyntaxTree
{
    private Dictionary<SyntaxNode, SyntaxNode?>? _parents;

    private delegate void ParseHandler(SyntaxTree syntaxTree,
                                       out CompilationUnitSyntax root,
                                       out ImmutableArray<Diagnostic> diagnostics);

    private SyntaxTree(SourceText text, ParseHandler handler)
    {
        Text = text;

        handler(
            this, out CompilationUnitSyntax root, out ImmutableArray<Diagnostic> diagnostics);

        Diagnostics = diagnostics;
        Root = root;
    }

    /// <summary>
    /// </summary>
    public SourceText Text { get; }

    /// <summary>
    /// </summary>
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    /// <summary>
    /// </summary>
    public CompilationUnitSyntax Root { get; }

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static SyntaxTree Load(string fileName)
    {
        string text = File.ReadAllText(fileName);
        SourceText sourceText = SourceText.From(text, fileName);
        return Parse(sourceText);
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static SyntaxTree Parse(string text)
    {
        SourceText sourceText = SourceText.From(text);
        return Parse(sourceText);
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static SyntaxTree Parse(SourceText text)
    {
        static void Parse(SyntaxTree syntaxTree, out CompilationUnitSyntax root, out ImmutableArray<Diagnostic> diagnostics)
        {
            Parser parser = new Parser(syntaxTree);
            root = parser.ParseCompilationUnit();
            diagnostics = parser.Diagnostics.ToImmutableArray();
        }

        return new SyntaxTree(text, Parse);
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="includeEndOfFile"></param>
    /// <returns></returns>
    public static ImmutableArray<SyntaxToken> ParseTokens(
        string text, bool includeEndOfFile = false)
    {
        SourceText sourceText = SourceText.From(text);
        return ParseTokens(sourceText, includeEndOfFile);
    }


    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="diagnostics"></param>
    /// <param name="includeEndOfFile"></param>
    /// <returns></returns>
    public static ImmutableArray<SyntaxToken> ParseTokens(
        string text, out ImmutableArray<Diagnostic> diagnostics, bool includeEndOfFile = false)
    {
        SourceText sourceText = SourceText.From(text);
        return ParseTokens(sourceText, out diagnostics, includeEndOfFile);
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="includeEndOfFile"></param>
    /// <returns></returns>
    public static ImmutableArray<SyntaxToken> ParseTokens(
        SourceText text, bool includeEndOfFile = false)
    {
        return ParseTokens(text, out _, includeEndOfFile);
    }


    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <param name="diagnostics"></param>
    /// <param name="includeEndOfFile"></param>
    /// <returns></returns>
    public static ImmutableArray<SyntaxToken> ParseTokens(
        SourceText text, out ImmutableArray<Diagnostic> diagnostics, bool includeEndOfFile = false)
    {
        List<SyntaxToken> tokens = new List<SyntaxToken>();

        void ParseTokens(SyntaxTree st, out CompilationUnitSyntax root, out ImmutableArray<Diagnostic> d)
        {
            Lexer l = new Lexer(st);
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

        SyntaxTree syntaxTree = new SyntaxTree(text, ParseTokens);
        diagnostics = syntaxTree.Diagnostics;
        return tokens.ToImmutableArray();
    }

    internal SyntaxNode? GetParent(SyntaxNode syntaxNode)
    {
        if (_parents == null)
        {
            Dictionary<SyntaxNode, SyntaxNode?> parents = CreateParentsDictionary(Root);
            Interlocked.CompareExchange(ref _parents, parents, null);
        }

        return _parents[syntaxNode];
    }

    private Dictionary<SyntaxNode, SyntaxNode?> CreateParentsDictionary(CompilationUnitSyntax root)
    {
        Dictionary<SyntaxNode, SyntaxNode?> result = new Dictionary<SyntaxNode, SyntaxNode?>
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
