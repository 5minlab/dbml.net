using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a database provider project setting clause.
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
    /// Gets the syntax kind of the database provider project setting clause <see cref="SyntaxKind.DatabaseProviderProjectSettingClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.DatabaseProviderProjectSettingClause;

    /// <summary>
    /// Gets the database provider keyword.
    /// </summary>
    public SyntaxToken DatabaseProviderKeyword { get; }

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
        yield return DatabaseProviderKeyword;
        yield return ColonToken;
        yield return ValueToken;
    }
}
