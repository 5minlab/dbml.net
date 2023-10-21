using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a project setting clause in the syntax tree.
/// </summary>
public sealed class NoteProjectSettingClause : ProjectSettingClause
{
    internal NoteProjectSettingClause(
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
    /// Gets the syntax kind of the project setting clause <see cref="SyntaxKind.NoteProjectSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NoteProjectSettingClause;

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

    /// <summary>
    /// Gets the children of the note project setting.
    /// </summary>
    /// <returns>The children of the note project setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NoteKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
