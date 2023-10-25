using System.Collections.Generic;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a enum identifier clause in the syntax tree.
/// </summary>
public sealed class EnumIdentifierClause : SyntaxNode
{
    internal EnumIdentifierClause(
        SyntaxTree syntaxTree,
        SyntaxToken? schemaIdentifier,
        SyntaxToken? dotToken,
        SyntaxToken enumIdentifier)
        : base(syntaxTree)
    {
        SchemaIdentifier = schemaIdentifier;
        DotToken = dotToken;
        EnumIdentifier = enumIdentifier;
    }

    /// <summary>
    /// Gets the syntax kind of the enum identifier clause <see cref="SyntaxKind.EnumIdentifierClause"/>.
    /// </summary>
    public override SyntaxKind Kind => SyntaxKind.EnumIdentifierClause;

    /// <summary>
    /// Gets the schema identifier.
    /// </summary>
    public SyntaxToken? SchemaIdentifier { get; }

    /// <summary>
    /// Gets the dot token.
    /// </summary>
    public SyntaxToken? DotToken { get; }

    /// <summary>
    /// Gets the enum identifier.
    /// </summary>
    public SyntaxToken EnumIdentifier { get; }

    /// <summary>
    /// Gets the children of the enum identifier.
    /// </summary>
    /// <returns>The children of the enum identifier.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        if (SchemaIdentifier is not null) yield return SchemaIdentifier;
        if (DotToken is not null) yield return DotToken;
        yield return EnumIdentifier;
    }
}
