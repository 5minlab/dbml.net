using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_ProjectDeclaration_With_Name_Identifier()
    {
        SyntaxKind projectNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string projectNameText = $"{randomText}";
        object? projectNameValue = null;
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_ProjectDeclaration_With_Name_QuotationMarksString()
    {
        SyntaxKind projectNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string projectNameText = $"\"{randomText}\"";
        object? projectNameValue = randomText;
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_ProjectDeclaration_With_Name_SingleQuotationMarksString()
    {
        SyntaxKind projectNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string projectNameText = $"\'{randomText}\'";
        object? projectNameValue = randomText;
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_ProjectDeclaration_With_Empty_Body()
    {
        SyntaxKind projectNameKind = SyntaxKind.IdentifierToken;
        string randomText = CreateRandomString();
        string projectNameText = randomText;
        object? projectNameValue = null;
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertNode(SyntaxKind.BlockStatement);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_DatabaseProviderDeclaration_With_QuotationMarksString_Value()
    {
        SyntaxKind providerKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string providerText = $"\"{randomText}\"";
        object? providerValue = randomText;
        string text = $"database_type: {providerText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.DatabaseProviderDeclarationStatement);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }

    [Fact]
    public void Parse_DatabaseProviderDeclaration_With_SingleQuotationMarksString_Value()
    {
        SyntaxKind providerKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = CreateRandomMultiWordString();
        string providerText = $"\'{randomText}\'";
        object? providerValue = randomText;
        string text = $"database_type: {providerText}";

        StatementSyntax statement = ParseStatement(text);

        using AssertingEnumerator e = new AssertingEnumerator(statement);
        e.AssertNode(SyntaxKind.DatabaseProviderDeclarationStatement);
        e.AssertToken(SyntaxKind.DatabaseTypeKeyword, "database_type");
        e.AssertToken(SyntaxKind.ColonToken, ":");
        e.AssertToken(providerKind, providerText, providerValue);
    }
}
