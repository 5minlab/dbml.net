using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NoteDeclarationStatement;

    /// <summary>
    /// </summary>
    public SyntaxToken NoteKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken Note { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NoteKeyword;
        yield return ColonToken;
        yield return Note;
    }
}
