using System.Collections.Generic;
using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ProjectSettingListClause;

    /// <summary>
    /// </summary>
    public ImmutableArray<ProjectSettingClause> Settings { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        foreach (SyntaxNode setting in Settings)
            yield return setting;
    }
}
