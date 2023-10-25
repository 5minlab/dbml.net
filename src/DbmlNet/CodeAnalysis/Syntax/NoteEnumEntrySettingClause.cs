using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a note enum entry setting clause in the syntax tree.
/// </summary>
public sealed class NoteEnumEntrySettingClause : EnumEntrySettingClause
{
    internal NoteEnumEntrySettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken noteKeyword,
        SyntaxToken colonToken,
        SyntaxToken valueToken)
        : base(syntaxTree)
    {
        NoteKeyword = noteKeyword;
        ColonToken = colonToken;
        ValueToken = valueToken;
        SettingName = noteKeyword.Text;
    }

    /// <summary>
    /// Gets the syntax kind of the note enum entry setting clause <see cref="SyntaxKind.NoteEnumEntrySettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.NoteEnumEntrySettingClause;

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public override string SettingName { get; }

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
    /// Gets the children of the note enum entry setting.
    /// </summary>
    /// <returns>The children of the note enum entry setting.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return NoteKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
