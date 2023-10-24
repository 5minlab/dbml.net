namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a enum entry setting clause in the syntax tree.
/// </summary>
public abstract class EnumEntrySettingClause : SyntaxNode
{
    private protected EnumEntrySettingClause(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }

    /// <summary>
    /// Gets the setting name.
    /// </summary>
    public abstract string SettingName { get; }
}
