using System.Collections.Generic;
using System.Linq;

using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public abstract class SyntaxNode
{
    private protected SyntaxNode(SyntaxTree syntaxTree)
    {
        SyntaxTree = syntaxTree;
    }

    /// <summary>
    /// </summary>
    public SyntaxTree SyntaxTree { get; }

    /// <summary>
    /// </summary>
    public SyntaxNode? Parent => SyntaxTree.GetParent(this);

    /// <summary>
    /// </summary>
    public abstract SyntaxKind Kind { get; }

    /// <summary>
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
    /// </summary>
    public TextLocation Location => new TextLocation(SyntaxTree.Text, Span);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerable<SyntaxNode> GetChildren();

    /// <summary>
    /// </summary>
    /// <returns></returns>
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
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SyntaxNode> Ancestors()
    {
        return AncestorsAndSelf().Skip(1);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public SyntaxToken GetLastToken()
    {
        if (this is SyntaxToken token)
            return token;

        // A syntax node should always contain at least 1 token.
        return GetChildren().Last().GetLastToken();
    }
}
