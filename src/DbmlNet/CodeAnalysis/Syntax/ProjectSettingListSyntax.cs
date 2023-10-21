using System.Collections.Generic;
using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a list of settings in the syntax tree.
/// </summary>
public sealed class ProjectSettingListSyntax : SyntaxNode
{
    internal ProjectSettingListSyntax(
        SyntaxTree syntaxTree,
        ImmutableArray<ProjectSettingClause> settings)
        : base(syntaxTree)
    {
        Settings = settings;
    }

    /// <summary>
    /// Gets the syntax kind of the project setting list clause <see cref="SyntaxKind.ProjectSettingListClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ProjectSettingListClause;

    /// <summary>
    /// Gets the settings.
    /// </summary>
    public ImmutableArray<ProjectSettingClause> Settings { get; }

    /// <summary>
    /// Gets the children of the project setting list.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        foreach (SyntaxNode setting in Settings)
            yield return setting;
    }
}
