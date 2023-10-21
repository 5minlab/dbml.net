namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column setting clause in the syntax tree.
/// </summary>
public abstract class ColumnSettingClause : SyntaxNode
{
    private protected ColumnSettingClause(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public abstract string SettingName { get; }
}
