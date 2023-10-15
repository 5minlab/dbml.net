using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DbmlNet.CodeAnalysis.Text;
using DbmlNet.IO;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a syntax node.
/// </summary>
public abstract class SyntaxNode
{
    private protected SyntaxNode(SyntaxTree syntaxTree)
    {
        SyntaxTree = syntaxTree;
    }

    /// <summary>
    /// Gets the syntax tree of the current syntax node.
    /// </summary>
    public SyntaxTree SyntaxTree { get; }

    /// <summary>
    /// Gets the parent of the current syntax node.
    /// </summary>
    public SyntaxNode? Parent => SyntaxTree.GetParent(this);

    /// <summary>
    /// Gets the kind of the syntax node.
    /// </summary>
    public abstract SyntaxKind Kind { get; }

    /// <summary>
    /// Gets the span of the syntax node.
    /// </summary>
    public virtual TextSpan Span
    {
        get
        {
            TextSpan first = GetChildren().First().Span;
            TextSpan last = GetChildren().Last().Span;
            return TextSpan.FromBounds(first.Start, last.End);
        }
    }

    /// <summary>
    /// Gets the text location of the syntax node.
    /// </summary>
    public TextLocation Location => new TextLocation(SyntaxTree.Text, Span);

    /// <summary>
    /// Gets the child syntax nodes of the current syntax node.
    /// </summary>
    /// <returns>An enumerable collection of child syntax nodes.</returns>
    public abstract IEnumerable<SyntaxNode> GetChildren();

    /// <summary>
    /// Gets the ancestors and self of the current syntax node.
    /// </summary>
    /// <returns>An enumerable collection of syntax nodes representing the current syntax node and its ancestors.</returns>
    public IEnumerable<SyntaxNode> AncestorsAndSelf()
    {
        SyntaxNode? node = this;
        while (node != null)
        {
            yield return node;
            node = node.Parent;
        }
    }

    /// <summary>
    /// Gets the ancestors of the current syntax node.
    /// </summary>
    /// <returns>An enumerable collection of syntax nodes representing the current syntax node and its ancestors.</returns>
    public IEnumerable<SyntaxNode> Ancestors()
    {
        return AncestorsAndSelf().Skip(1);
    }

    /// <summary>
    /// Gets the last token of the current syntax node.
    /// </summary>
    /// <returns>The last token of the current syntax node.</returns>
    public SyntaxToken GetLastToken()
    {
        if (this is SyntaxToken token)
            return token;

        // A syntax node should always contain at least 1 token.
        return GetChildren().Last().GetLastToken();
    }

    /// <summary>
    /// Writes a tree view string of the current <see cref="SyntaxNode"/> instance to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
    /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
    public void WriteTo(TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);
        PrintSyntaxTo(writer, this);
    }

    private static void PrintSyntaxTo(
        TextWriter writer,
        SyntaxNode node,
        string indent = "",
        bool isLast = true)
    {
        writer.WritePunctuation(indent);
        writer.WritePunctuation(isLast ? "└── " : "├── ");

        SyntaxToken? token = node as SyntaxToken;
        string tokenRangeText = $"[{token?.Start}..{token?.End}]";

        if (node.Kind.IsKeyword())
        {
            writer.WriteInformation("Keyword");
            writer.WriteSpace();
            writer.WritePunctuation(tokenRangeText);
            writer.WritePunctuation(":");
            writer.WriteSpace();
            writer.WriteKeyword($"{node.Kind.GetKnownText()}");
        }
        else if (node.Kind.IsToken())
        {
            if (node.Kind == SyntaxKind.EndOfFileToken)
            {
                writer.WriteInformation($"{node.Kind}");
                writer.WriteSpace();
                writer.WritePunctuation(tokenRangeText);
            }
            else if (node.Kind == SyntaxKind.IdentifierToken)
            {
                writer.WriteInformation("Identifier");
                writer.WriteSpace();
                writer.WritePunctuation(tokenRangeText);
                writer.WritePunctuation(":");
                writer.WriteSpace();

                if (token?.Value is not null)
                    writer.WriteIdentifier($"{token.Value}");
                else if (!string.IsNullOrEmpty(token?.Text))
                    writer.WriteIdentifier(token.Text);
                else
                    writer.WriteError(node.Kind.ToString());
            }
            else
            {
                string tokenText = $"{node.Kind}"[..^5];
                writer.WriteInformation(tokenText);
                writer.WriteSpace();
                writer.WritePunctuation(tokenRangeText);
                writer.WritePunctuation(":");
                writer.WriteSpace();

                if (token?.Value is not null)
                    writer.WriteIdentifier($"{token.Value}");
                else if (!string.IsNullOrEmpty(token?.Text))
                    writer.WriteKeyword(token.Text);
                else
                    writer.WriteError(node.Kind.ToString());
            }
        }
        else
        {
            // Nodes && Statements
            bool nodeIsExpression = node.Kind.IsSyntaxExpression();
            bool nodeIsStatement = node.Kind.IsSyntaxStatement();
            bool nodeIsMember = node.Kind.IsSyntaxMember();
            string kindText = $"{node.Kind}";

            string displayText =
                nodeIsExpression ? $"Expression: {kindText[..^10]}"
                : nodeIsStatement ? $"Statement: {kindText[..^9]}"
                : nodeIsMember ? kindText[..^6]
                : kindText;

            writer.WriteSuccess(displayText);
        }

        writer.WriteLine();

        indent += isLast ? $"   " : "│  ";

        SyntaxNode? lastChild = node.GetChildren().LastOrDefault();

        foreach (SyntaxNode child in node.GetChildren())
            PrintSyntaxTo(writer, child, indent, child == lastChild);
    }

    /// <summary>
    /// Return a tree view string of the current <see cref="SyntaxNode"/>.
    /// </summary>
    /// <returns>A tree view string of the current <see cref="SyntaxNode"/>.</returns>
    public override string ToString()
    {
        using StringWriter writer = new();
        WriteTo(writer);
        return writer.ToString();
    }
}
