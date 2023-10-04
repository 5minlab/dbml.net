using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public sealed class DatabaseProviderProjectSettingClause : ProjectSettingClause
{
    internal DatabaseProviderProjectSettingClause(
        SyntaxTree syntaxTree,
        SyntaxToken databaseProviderKeyword,
        SyntaxToken colonToken,
        SyntaxToken valueToken)
        : base(syntaxTree)
    {
        DatabaseProviderKeyword = databaseProviderKeyword;
        ColonToken = colonToken;
        ValueToken = valueToken;
    }

    /// <summary>
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.DatabaseProviderProjectSettingClause;

    /// <summary>
    /// </summary>
    public SyntaxToken DatabaseProviderKeyword { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ColonToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken ValueToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return DatabaseProviderKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
