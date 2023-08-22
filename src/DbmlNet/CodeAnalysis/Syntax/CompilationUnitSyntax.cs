using System.Collections.Generic;
using System.Collections.Immutable;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
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
    /// </summary>
    public ImmutableArray<MemberSyntax> Members { get; }

    /// <summary>
    /// </summary>
    public SyntaxToken EndOfFileToken { get; }

    /// <inherits/>
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        foreach (MemberSyntax member in Members)
            yield return member;
        yield return EndOfFileToken;
    }
}
