using System.Collections.Generic;
using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ProjectDeclarationMember;

    /// <summary>
    /// </summary>
    public SyntaxToken ProjectKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken IdentifierToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken OpenBraceToken { get; }

    /// <summary>
    /// </summary>
    public ProjectSettingListSyntax Settings { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken CloseBraceToken { get; }

    /// <inherits/>
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
