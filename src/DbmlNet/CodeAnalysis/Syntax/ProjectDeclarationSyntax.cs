using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a project declaration in the syntax tree.
/// </summary>
public sealed class ProjectDeclarationSyntax : MemberSyntax
{
    internal ProjectDeclarationSyntax(
        SyntaxTree syntaxTree,
        SyntaxToken projectKeyword,
        SyntaxToken identifierToken,
        SyntaxToken openBraceToken,
        ProjectSettingListSyntax settings,
        SyntaxToken closeBraceToken)
        : base(syntaxTree)
    {
        ProjectKeyword = projectKeyword;
        IdentifierToken = identifierToken;
        OpenBraceToken = openBraceToken;
        Settings = settings;
        CloseBraceToken = closeBraceToken;
    }

    /// <summary>
    /// Gets the syntax kind of the project declaration <see cref="SyntaxKind.ProjectDeclarationMember"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ProjectDeclarationMember;

    /// <summary>
    /// Gets the project keyword.
    /// </summary>
    public SyntaxToken ProjectKeyword { get; }

    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// Gets the open brace token.
    /// </summary>
    public SyntaxToken OpenBraceToken { get; }

    /// <summary>
    /// Gets the project setting list.
    /// </summary>
    public ProjectSettingListSyntax Settings { get; }

    /// <summary>
    /// Gets the close brace token.
    /// </summary>
    public SyntaxToken CloseBraceToken { get; }

    /// <summary>
    /// Gets the children of the project declaration.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ProjectKeyword;
        yield return IdentifierToken;
        yield return OpenBraceToken;
        foreach (SyntaxNode setting in Settings.GetChildren())
            yield return setting;
        yield return CloseBraceToken;
    }
}
