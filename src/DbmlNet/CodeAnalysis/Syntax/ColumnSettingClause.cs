namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column setting clause in the syntax tree.
/// </summary>
public abstract class ColumnSettingClause : SyntaxNode
{
    private protected ColumnSettingClause(SyntaxTree syntaxTree, string settingName)
        : base(syntaxTree)
    {
        SettingName = settingName;
    }

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public string SettingName { get; }
}
