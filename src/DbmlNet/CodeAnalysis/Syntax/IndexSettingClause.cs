namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a index setting clause in the syntax tree.
/// </summary>
public abstract class IndexSettingClause : SyntaxNode
{
    private protected IndexSettingClause(SyntaxTree syntaxTree, string settingName)
        : base(syntaxTree)
    {
        SettingName = settingName;
    }

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public string SettingName { get; }
}
