using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a note declaration statement in the syntax tree.
/// </summary>
public sealed class NoteDeclarationSyntax : StatementSyntax
{
    internal NoteDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken noteKeyword,
        SyntaxToken colonKeyword,
        SyntaxToken note)
        : base(syntaxTree)
    {
        NoteKeyword = noteKeyword;
        ColonToken = colonKeyword;
        Note = note;
    }

    /// <summary>
    /// Gets the syntax kind of the note declaration <see cref="SyntaxKind.NoteDeclarationStatement"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NoteDeclarationStatement;

    /// <summary>
    /// Gets the note keyword.
    /// </summary>
    public SyntaxToken NoteKeyword { get; }

    /// <summary>
    /// Gets the colon keyword.
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// Gets the note.
    /// </summary>
    public SyntaxToken Note { get; }

    /// <summary>
    /// Gets the children of the note declaration.
    /// </summary>
    /// <returns>The children of the note declaration.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NoteKeyword;
        yield return ColonToken;
        yield return Note;
    }
}
