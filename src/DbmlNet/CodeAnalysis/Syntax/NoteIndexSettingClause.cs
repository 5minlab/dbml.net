using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a index setting clause in the syntax tree.
/// </summary>
public sealed class NoteIndexSettingClause : IndexSettingClause
{
    internal NoteIndexSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken noteKeyword,
        SyntaxToken colonToken,
        SyntaxToken valueToken)
        : base(syntaxTree)
    {
        NoteKeyword = noteKeyword;
        ColonToken = colonToken;
        ValueToken = valueToken;
    }

    /// <summary>
    /// Gets the syntax kind of the index setting clause <see cref="SyntaxKind.NoteIndexSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NoteIndexSettingClause;

    /// <summary>
    /// Gets the note keyword.
    /// </summary>
    public SyntaxToken NoteKeyword { get; }

    /// <summary>
    /// Gets the colon token.
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// Gets the value token.
    /// </summary>
    public SyntaxToken ValueToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NoteKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
