namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents an index declaration statement.
/// </summary>
public abstract class IndexDeclarationStatementSyntax : StatementSyntax
{
    private protected IndexDeclarationStatementSyntax(SyntaxTree syntaxTree)
        : base(syntaxTree)
    {
    }

    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    public abstract string IdentifierName { get; }
}
