using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.ColumnTypeParenthesizedIdentifierClause;

    /// <summary>
    /// </summary>
    public SyntaxToken ColumnTypeIdentifier { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken OpenParenthesisToken { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken VariableLengthIdentifier { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken CloseParenthesisToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return ColumnTypeIdentifier;
        yield return OpenParenthesisToken;
        yield return VariableLengthIdentifier;
        yield return CloseParenthesisToken;
    }
}
