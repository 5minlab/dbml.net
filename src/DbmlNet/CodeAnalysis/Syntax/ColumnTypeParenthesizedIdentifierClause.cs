using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a column type clause in the syntax tree.
/// </summary>
public sealed class ColumnTypeParenthesizedIdentifierClause : ColumnTypeClause
{
    internal ColumnTypeParenthesizedIdentifierClause(
        SyntaxTree syntaxTree,
        SyntaxToken columnTypeIdentifier,
        SyntaxToken openParenthesisToken,
        SyntaxToken valueTypeIdentifier,
        SyntaxToken closeParenthesisToken)
        : base(syntaxTree)
    {
        ColumnTypeIdentifier = columnTypeIdentifier;
        OpenParenthesisToken = openParenthesisToken;
        VariableLengthIdentifier = valueTypeIdentifier;
        CloseParenthesisToken = closeParenthesisToken;
    }

    /// <summary>
    /// Gets the syntax kind of the column type identifier clause <see cref="SyntaxKind.ColumnTypeParenthesizedIdentifierClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnTypeParenthesizedIdentifierClause;

    /// <summary>
    /// Gets the column type identifier.
    /// </summary>
    public SyntaxToken ColumnTypeIdentifier { get; }

    /// <summary>
    /// Gets the open parenthesis token.
    /// </summary>
    public SyntaxToken OpenParenthesisToken { get; }

    /// <summary>
    /// Gets the variable length identifier.
    /// </summary>
    public SyntaxToken VariableLengthIdentifier { get; }

    /// <summary>
    /// Gets the close parenthesis token.
    /// </summary>
    public SyntaxToken CloseParenthesisToken { get; }

    /// <summary>
    /// Gets the children of the column type identifier.
    /// </summary>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ColumnTypeIdentifier;
        yield return OpenParenthesisToken;
        yield return VariableLengthIdentifier;
        yield return CloseParenthesisToken;
    }
}
