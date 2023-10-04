using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_CompilationUnit_With_No_Members()
    {
        string text = "";

        SyntaxTree syntaxTree = SyntaxTree.Parse(text);

        using AssertingEnumerator e = new AssertingEnumerator(syntaxTree.Root);
        e.AssertNode(SyntaxKind.CompilationUnitMember);
        e.AssertToken(SyntaxKind.EndOfFileToken, "");
    }
}
