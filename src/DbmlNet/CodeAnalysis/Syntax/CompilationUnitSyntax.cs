using System.Collections.Generic;
using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a compilation unit in the syntax tree.
/// </summary>
public sealed class CompilationUnitSyntax : SyntaxNode
{
    internal CompilationUnitSyntax(
        SyntaxTree syntaxTree,
        ImmutableArray<MemberSyntax> members,
        SyntaxToken endOfFileToken)
        : base(syntaxTree)
    {
        Members = members;
        EndOfFileToken = endOfFileToken;
    }

    /// <inheritdoc/>
    public override SyntaxKind Kind => SyntaxKind.CompilationUnitMember;

    /// <summary>
    /// Gets the members of the compilation unit.
    /// </summary>
    public ImmutableArray<MemberSyntax> Members { get; }

    /// <summary>
    /// Gets the end of file token.
    /// </summary>
    public SyntaxToken EndOfFileToken { get; }

    /// <summary>
    /// Gets the children of the compilation unit.
    /// </summary>
    /// <returns>The children of the compilation unit.</returns>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        foreach (MemberSyntax member in Members)
            yield return member;
        yield return EndOfFileToken;
    }
}
