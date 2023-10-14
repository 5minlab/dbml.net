using System.Collections.Generic;
using System.Linq;

using DbmlNet.CodeAnalysis.Text;

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
}
