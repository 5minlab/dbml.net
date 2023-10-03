using System;
using System.Collections.Generic;
using System.Linq;

using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

internal sealed class AssertingEnumerator : IDisposable
{
    private readonly IEnumerator<SyntaxNode> _enumerator;
    private bool _hasErrors;

    public AssertingEnumerator(SyntaxNode node)
    {
        _enumerator = Flatten(node).GetEnumerator();
    }

    public SyntaxNode? Node => _enumerator.Current;

    private bool MarkFailed()
    {
        _hasErrors = true;
        return false;
    }

    public void Dispose()
    {
        if (!_hasErrors)
            Assert.False(_enumerator.MoveNext(), $"Unhandled node kind <{_enumerator.Current.Kind}>.");

        _enumerator.Dispose();
    }

    private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
    {
        Stack<SyntaxNode> stack = new Stack<SyntaxNode>();
        stack.Push(node);

        while (stack.Count > 0)
        {
            SyntaxNode n = stack.Pop();
            yield return n;

            foreach (SyntaxNode child in n.GetChildren().Reverse())
                stack.Push(child);
        }
    }

    public void AssertNode(SyntaxKind kind)
    {
        try
        {
            Assert.True(_enumerator.MoveNext(), $"There are zero nodes left, expected <{kind}>.");
            Assert.Equal(kind, _enumerator.Current.Kind);
            Assert.IsNotType<SyntaxToken>(_enumerator.Current);
        }
        catch when (MarkFailed())
        {
            throw;
        }
    }

    public void AssertToken(SyntaxKind kind, string text, object? value = null, bool isMissing = false)
    {
        try
        {
            Assert.True(_enumerator.MoveNext(), $"There are zero tokens left, expected <{kind}>.");
            Assert.Equal(kind, _enumerator.Current.Kind);
            SyntaxToken token = Assert.IsType<SyntaxToken>(_enumerator.Current);
            Assert.Equal(text, token.Text);
            Assert.Equal(value, token.Value);
            if (isMissing)
                Assert.True(token.IsMissing, $"Token <{kind}> should be missing.");
            else
                Assert.False(token.IsMissing, $"Token <{kind}> should not be missing.");
        }
        catch when (MarkFailed())
        {
            throw;
        }
    }
}
