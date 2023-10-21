using DbmlNet.CodeAnalysis.Syntax;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public partial class ParserTests
{
    [Fact]
    public void Parse_ProjectDeclaration_With_Name_Identifier()
    {
        SyntaxKind projectNameKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
        string projectNameText = $"{randomText}";
        object? projectNameValue = null;
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Theory]
    [MemberData(nameof(GetSyntaxKeywordTokensData))]
    public void Parse_ProjectDeclaration_With_Name_Keyword(
        SyntaxKind projectNameKind,
        string projectNameText,
        object? projectNameValue)
    {
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_ProjectDeclaration_With_Name_QuotationMarksString()
    {
        SyntaxKind projectNameKind = SyntaxKind.QuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string projectNameText = $"\"{randomText}\"";
        object? projectNameValue = randomText;
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_ProjectDeclaration_With_Name_SingleQuotationMarksString()
    {
        SyntaxKind projectNameKind = SyntaxKind.SingleQuotationMarksStringToken;
        string randomText = DataGenerator.CreateRandomMultiWordString();
        string projectNameText = $"\'{randomText}\'";
        object? projectNameValue = randomText;
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_ProjectDeclaration_With_Empty_Settings()
    {
        SyntaxKind projectNameKind = SyntaxKind.IdentifierToken;
        string randomText = DataGenerator.CreateRandomString();
        string projectNameText = randomText;
        object? projectNameValue = null;
        string text = $"Project {projectNameText} " + "{ }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(projectNameKind, projectNameText, projectNameValue);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }

    [Fact]
    public void Parse_ProjectDeclaration_With_UnknownSetting()
    {
        SyntaxKind tableNameKind = SyntaxKind.IdentifierToken;
        string tableNameText = DataGenerator.CreateRandomString();
        object? tableNameValue = null;
        SyntaxKind settingNameKind = SyntaxKind.IdentifierToken;
        string settingNameText = DataGenerator.CreateRandomString();
        string settingText = settingNameText;
        string text = $"Project {tableNameText} " + "{ " + settingText + " }";

        MemberSyntax member = ParseMember(text);

        using AssertingEnumerator e = new AssertingEnumerator(member);
        e.AssertNode(SyntaxKind.ProjectDeclarationMember);
        e.AssertToken(SyntaxKind.ProjectKeyword, "Project");
        e.AssertToken(tableNameKind, tableNameText, tableNameValue);
        e.AssertToken(SyntaxKind.OpenBraceToken, "{");
        e.AssertNode(SyntaxKind.UnknownProjectSettingClause);
        e.AssertToken(settingNameKind, settingNameText);
        e.AssertToken(SyntaxKind.CloseBraceToken, "}");
    }
}
