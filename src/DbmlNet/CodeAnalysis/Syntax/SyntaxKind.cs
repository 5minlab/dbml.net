namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents all available syntax kinds.
/// </summary>
public enum SyntaxKind
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member.
    BadToken,

    // Trivia
    WhitespaceTrivia,

    // Tokens
    EndOfFileToken,
    NumberToken,
    DotToken,
    MinusToken,
    PlusToken,
    SlashToken,
    StarToken,
    CommaToken,
    ColonToken,
    LessToken,
    LessGraterToken,
    GraterToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    OpenBraceToken,
    CloseBraceToken,
    OpenBracketToken,
    CloseBracketToken,
    QuotationMarksStringToken,
    SingleQuotationMarksStringToken,
    IdentifierToken,

    // Keywords
    DatabaseTypeKeyword,
    DefaultKeyword,
    FalseKeyword,
    IncrementKeyword,
    IndexesKeyword,
    KeyKeyword,
    NameKeyword,
    NoteKeyword,
    NotKeyword,
    NullKeyword,
    PkKeyword,
    PrimaryKeyword,
    ProjectKeyword,
    RefKeyword,
    TableKeyword,
    TrueKeyword,
    TypeKeyword,
    UniqueKeyword,

    // Nodes
    CompilationUnitMember,
    ProjectDeclarationMember,
    TableDeclarationMember,
    GlobalStatementMember,

    // Project nodes
    ProjectSettingListClause,
    DatabaseProviderProjectSettingClause,
    NoteProjectSettingClause,
    UnknownProjectSettingClause,

    // Column nodes
    ColumnIdentifierClause,
    ColumnTypeIdentifierClause,
    ColumnTypeParenthesizedIdentifierClause,
    ColumnSettingListClause,
    PrimaryKeyColumnSettingClause,
    PkColumnSettingClause,
    NullColumnSettingClause,
    NotNullColumnSettingClause,
    UniqueColumnSettingClause,
    IncrementColumnSettingClause,
    DefaultColumnSettingClause,
    NoteColumnSettingClause,
    RelationshipColumnSettingClause,
    RelationshipConstraintClause,
    UnknownColumnSettingClause,

    // Statements
    BlockStatement,
    NoteDeclarationStatement,
    ColumnDeclarationStatement,
    DatabaseProviderDeclarationStatement,
    IndexesDeclarationStatement,
    SingleFieldIndexDeclarationStatement,
    CompositeIndexDeclarationStatement,
    ExpressionStatement,

    // Expressions
    LiteralExpression,
    NameExpression,
    IndexSettingExpression,
    ParenthesizedExpression,

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member.
}
