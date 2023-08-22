using System;
using System.IO;
using System.Linq;

using DbmlNet.CodeAnalysis.Syntax;

namespace DbmlNet.IO;

internal static class SyntaxTreeExtensions
{
    internal static void PrintSyntaxTo(this SyntaxTree syntaxTree, TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.PrintSyntaxTo(syntaxTree.Root);
    }

    private static void PrintSyntaxTo(
        this TextWriter writer,
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
            writer.PrintSyntaxTo(child, indent, child == lastChild);
    }
}
