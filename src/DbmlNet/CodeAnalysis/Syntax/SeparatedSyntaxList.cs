using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a list of expressions in the syntax tree.
/// </summary>
public abstract class SeparatedSyntaxList
{
    private protected SeparatedSyntaxList()
    {
    }

    /// <summary>
    /// Returns an immutable array of syntax nodes that includes both the nodes and the separators.
    /// </summary>
    /// <returns>An immutable array of syntax nodes.</returns>
    public abstract ImmutableArray<SyntaxNode> GetWithSeparators();
}

/// <summary>
/// Represents a list of syntax nodes that includes both the nodes and the separators.
/// </summary>
/// <typeparam name="T">The type of the syntax nodes in the list.</typeparam>
public sealed class SeparatedSyntaxList<T> : SeparatedSyntaxList, IEnumerable<T>
    where T : SyntaxNode
{
    private readonly ImmutableArray<SyntaxNode> _nodesAndSeparators;

    internal SeparatedSyntaxList(ImmutableArray<SyntaxNode> nodesAndSeparators)
    {
        _nodesAndSeparators = nodesAndSeparators;
    }

    /// <summary>
    /// Gets an empty separated syntax list.
    /// </summary>
    public static readonly SeparatedSyntaxList<T> Empty = new(ImmutableArray<SyntaxNode>.Empty);

    /// <summary>
    /// Gets the number of nodes in the list.
    /// </summary>
    public int Count => (_nodesAndSeparators.Length + 1) / 2;

    /// <summary>
    /// Gets the node at the specified index.
    /// </summary>
    public T this[int index] => (T)_nodesAndSeparators[index * 2];

    /// <summary>
    /// Gets the separator at the specified index.
    /// </summary>
    /// <param name="index">The index of the separator.</param>
    /// <returns>The separator at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is out of range.</exception>
    public SyntaxToken GetSeparator(int index)
    {
        return index < 0 || index >= Count - 1
            ? throw new ArgumentOutOfRangeException(nameof(index))
            : (SyntaxToken)_nodesAndSeparators[index * 2 + 1];
    }

    /// <summary>
    /// Returns an immutable array of syntax nodes that includes both the nodes and the separators.
    /// </summary>
    /// <returns>An immutable array of syntax nodes.</returns>
    public override ImmutableArray<SyntaxNode> GetWithSeparators() => _nodesAndSeparators;

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return this[i];
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
