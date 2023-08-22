using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public abstract class SeparatedSyntaxList
{
    private protected SeparatedSyntaxList()
    {
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract ImmutableArray<SyntaxNode> GetWithSeparators();
}

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class SeparatedSyntaxList<T> : SeparatedSyntaxList, IEnumerable<T>
    where T : SyntaxNode
{
    private readonly ImmutableArray<SyntaxNode> _nodesAndSeparators;

    internal SeparatedSyntaxList(ImmutableArray<SyntaxNode> nodesAndSeparators)
    {
        _nodesAndSeparators = nodesAndSeparators;
    }

    /// <summary>
    /// </summary>
    public int Count => (_nodesAndSeparators.Length + 1) / 2;

    /// <summary>
    /// </summary>
    public T this[int index] => (T)_nodesAndSeparators[index * 2];

    /// <summary>
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public SyntaxToken GetSeparator(int index)
    {
        return index < 0 || index >= Count - 1
            ? throw new ArgumentOutOfRangeException(nameof(index))
            : (SyntaxToken)_nodesAndSeparators[index * 2 + 1];
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override ImmutableArray<SyntaxNode> GetWithSeparators() => _nodesAndSeparators;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
