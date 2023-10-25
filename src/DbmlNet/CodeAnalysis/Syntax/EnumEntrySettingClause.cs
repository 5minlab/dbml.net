namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a enum entry setting clause in the syntax tree.
/// </summary>
public abstract class EnumEntrySettingClause : SyntaxNode
{
    private protected EnumEntrySettingClause(SyntaxTree syntaxTree, string settingName)
        : base(syntaxTree)
    {
        SettingName = settingName;
    }

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public string SettingName { get; }
}
