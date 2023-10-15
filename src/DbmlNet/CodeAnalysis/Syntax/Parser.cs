using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

internal sealed class Parser
{
    private readonly SyntaxTree _syntaxTree;
    private readonly ImmutableArray<SyntaxToken> _tokens;
    private int _position;

    public Parser(SyntaxTree syntaxTree)
    {
        _syntaxTree = syntaxTree;

        ParseTokens(
            _syntaxTree,
            out ImmutableArray<SyntaxToken> tokens,
            out ImmutableArray<Diagnostic> diagnostics);

        _tokens = tokens;
        Diagnostics.AddRange(diagnostics);
    }

    static void ParseTokens(
        SyntaxTree syntaxTree,
        out ImmutableArray<SyntaxToken> tokens,
        out ImmutableArray<Diagnostic> diagnostics)
    {
        Lexer lexer = new Lexer(syntaxTree);

        ImmutableArray<SyntaxToken>.Builder tokenList =
            ImmutableArray.CreateBuilder<SyntaxToken>();

        ImmutableArray<SyntaxToken>.Builder badTokens =
            ImmutableArray.CreateBuilder<SyntaxToken>();

        SyntaxToken token;
        do
        {
            token = lexer.Lex();

            bool skipToken = token.Kind switch
            {
                SyntaxKind.BadToken => true,
                SyntaxKind.WhitespaceTrivia => true,
                _ => false
            };

            if (skipToken)
                badTokens.Add(token);
            else
                tokenList.Add(token);

        } while (token.Kind != SyntaxKind.EndOfFileToken);

        diagnostics = lexer.Diagnostics.ToImmutableArray();
        tokens = tokenList.ToImmutableArray();
    }

    public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

    private SyntaxToken Peek(int offset)
    {
        int index = _position + offset;
        return index < _tokens.Length
            ? _tokens[index]
            : _tokens[^1];
    }

    private SyntaxToken Current => Peek(offset: 0);
    private SyntaxToken Lookahead => Peek(offset: 1);

    private SyntaxToken NextToken()
    {
        SyntaxToken token = Current;
        _position++;
        return token;
    }

    private SyntaxToken MatchToken(SyntaxKind kind)
    {
        if (Current.Kind == kind)
            return NextToken();

        Diagnostics.ReportUnexpectedToken(Current.Location, Current.Kind, kind);
        return new SyntaxToken(_syntaxTree, kind, Current.Start, text: null, value: null);
    }

    /// <summary>
    /// Parses the compilation unit and returns the resulting <see cref="CompilationUnitSyntax"/>.
    /// </summary>
    /// <returns>The <see cref="CompilationUnitSyntax"/> object representing the parsed compilation unit.</returns>
    public CompilationUnitSyntax ParseCompilationUnit()
    {
        ImmutableArray<MemberSyntax> members = ParseMembers();
        SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
        return new CompilationUnitSyntax(_syntaxTree, members, endOfFileToken);
    }

    private ImmutableArray<MemberSyntax> ParseMembers()
    {
        ImmutableArray<MemberSyntax>.Builder members =
            ImmutableArray.CreateBuilder<MemberSyntax>();

        while (Current.Kind != SyntaxKind.EndOfFileToken)
        {
            SyntaxToken startToken = Current;

            MemberSyntax member = ParseMember();
            members.Add(member);

            // If ParseMember() did not consume any tokens,
            // we need to skip the current token and continue
            // in order to avoid an infinite loop.
            //
            // We don't need to report an error, because we'll
            // already tried to parse an expression statement
            // and reported one.
            if (Current == startToken)
                NextToken();
        }

        return members.ToImmutableArray();
    }

    private MemberSyntax ParseMember()
    {
        return Current.Kind switch
        {
            SyntaxKind.ProjectKeyword => ParseProjectDeclaration(),
            SyntaxKind.TableKeyword => ParseTableDeclaration(),
            _ => ParseGlobalStatement()
        };
    }

    private MemberSyntax ParseGlobalStatement()
    {
        StatementSyntax statement = ParseStatement();
        return new GlobalStatementSyntax(_syntaxTree, statement);
    }

    private MemberSyntax ParseProjectDeclaration()
    {
        SyntaxToken projectKeyword = MatchToken(SyntaxKind.ProjectKeyword);
        SyntaxToken identifier = Current.Kind switch
        {
            SyntaxKind.QuotationMarksStringToken
                => MatchToken(SyntaxKind.QuotationMarksStringToken),
            SyntaxKind.SingleQuotationMarksStringToken
                => MatchToken(SyntaxKind.SingleQuotationMarksStringToken),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };

        SyntaxToken openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);
        ProjectSettingListSyntax settings = ParseProjectSettingList();
        SyntaxToken closeBraceToken = MatchToken(SyntaxKind.CloseBraceToken);

        return new ProjectDeclarationSyntax(
            _syntaxTree, projectKeyword, identifier,
            openBraceToken, settings, closeBraceToken);
    }

    private ProjectSettingListSyntax ParseProjectSettingList()
    {
        ImmutableArray<ProjectSettingClause>.Builder settings =
            ImmutableArray.CreateBuilder<ProjectSettingClause>();

        while (Current.Kind != SyntaxKind.CloseBraceToken
            && Current.Kind != SyntaxKind.EndOfFileToken)
        {
            ProjectSettingClause settingClause = ParseProjectSettingClause();
            settings.Add(settingClause);
        }

        return new ProjectSettingListSyntax(_syntaxTree, settings.ToImmutableArray());
    }

    private ProjectSettingClause ParseProjectSettingClause()
    {
        switch (Current.Kind)
        {
            case SyntaxKind.DatabaseTypeKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
                return ParseDatabaseProviderProjectSetting();
            case SyntaxKind.NoteKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
                return ParseNoteProjectSetting();
            default:
                return ParseUnknownProjectSetting();
        }
    }

    private ProjectSettingClause ParseDatabaseProviderProjectSetting()
    {
        SyntaxToken databaseTypeKeyword = MatchToken(SyntaxKind.DatabaseTypeKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);

        SyntaxKind valueTokenKind = Current.Kind switch
        {
            SyntaxKind.QuotationMarksStringToken => SyntaxKind.QuotationMarksStringToken,
            _ => SyntaxKind.SingleQuotationMarksStringToken,
        };
        SyntaxToken valueToken = MatchToken(valueTokenKind);

        return new DatabaseProviderProjectSettingClause(_syntaxTree, databaseTypeKeyword, colonToken, valueToken);
    }

    private ProjectSettingClause ParseNoteProjectSetting()
    {
        SyntaxToken noteKeyword = MatchToken(SyntaxKind.NoteKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);

        SyntaxKind valueTokenKind = Current.Kind switch
        {
            SyntaxKind.QuotationMarksStringToken => SyntaxKind.QuotationMarksStringToken,
            _ => SyntaxKind.SingleQuotationMarksStringToken,
        };
        SyntaxToken valueToken = MatchToken(valueTokenKind);

        return new NoteProjectSettingClause(_syntaxTree, noteKeyword, colonToken, valueToken);
    }

    private ProjectSettingClause ParseUnknownProjectSetting()
    {
        void ReportUnknownProjectSetting(string settingName, int spanStart, int spanEnd)
        {
            SourceText text = _syntaxTree.Text;
            TextSpan span = new TextSpan(spanStart, length: spanEnd - spanStart);
            TextLocation location = new TextLocation(text, span);
            Diagnostics.ReportUnknownProjectSetting(location, settingName);
        }

        SyntaxToken identifierToken = Current.Kind.IsKeyword()
                ? NextToken()
                : MatchToken(SyntaxKind.IdentifierToken);

        if (Current.Kind == SyntaxKind.ColonToken)
        {
            SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);

#pragma warning disable CA1508 // Avoid dead conditional code
            SyntaxToken valueToken = Current.Kind switch
            {
                SyntaxKind.QuotationMarksStringToken => MatchToken(SyntaxKind.QuotationMarksStringToken),
                SyntaxKind.SingleQuotationMarksStringToken => MatchToken(SyntaxKind.SingleQuotationMarksStringToken),
                _ => MatchToken(SyntaxKind.IdentifierToken),
            };
#pragma warning restore CA1508 // Avoid dead conditional code

            ReportUnknownProjectSetting(identifierToken.Text, identifierToken.Start, valueToken.End);
            return new UnknownProjectSettingClause(_syntaxTree, identifierToken, colonToken, valueToken);
        }

        ReportUnknownProjectSetting(identifierToken.Text, identifierToken.Start, identifierToken.End);
        return new UnknownProjectSettingClause(_syntaxTree, identifierToken);
    }

    private MemberSyntax ParseTableDeclaration()
    {
        SyntaxToken tableKeyword = MatchToken(SyntaxKind.TableKeyword);
        SyntaxToken identifier = Current.Kind switch
        {
            SyntaxKind.QuotationMarksStringToken
                => MatchToken(SyntaxKind.QuotationMarksStringToken),
            SyntaxKind.SingleQuotationMarksStringToken
                => MatchToken(SyntaxKind.SingleQuotationMarksStringToken),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };
        TableSettingListSyntax? settingList = ParseOptionalTableSettingList();
        StatementSyntax body = ParseBlockStatement();
        return new TableDeclarationSyntax(_syntaxTree, tableKeyword, identifier, settingList, body);
    }

    private TableSettingListSyntax? ParseOptionalTableSettingList()
    {
        return Current.Kind == SyntaxKind.OpenBracketToken
            ? ParseTableSettingList()
            : null;
    }

    private TableSettingListSyntax ParseTableSettingList()
    {
        SyntaxToken openBracketToken = MatchToken(SyntaxKind.OpenBracketToken);

        SeparatedSyntaxList<TableSettingClause> settings =
            ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseBracketToken,
                separatorKind: SyntaxKind.CommaToken,
                parseExpression: ParseTableSettingClause);

        SyntaxToken closeBracketToken = MatchToken(SyntaxKind.CloseBracketToken);

        return new TableSettingListSyntax(_syntaxTree, openBracketToken, settings, closeBracketToken);
    }

    private TableSettingClause ParseTableSettingClause()
    {
        switch (Current.Kind)
        {
            default:
                return ParseUnknownTableSetting();
        }
    }

    private TableSettingClause ParseUnknownTableSetting()
    {
        SyntaxToken identifierToken = Current.Kind.IsKeyword()
                ? NextToken()
                : MatchToken(SyntaxKind.IdentifierToken);

        if (Current.Kind != SyntaxKind.ColonToken)
        {
            return new UnknownTableSettingClause(_syntaxTree, identifierToken);
        }

        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);

#pragma warning disable CA1508 // Avoid dead conditional code
        SyntaxToken valueToken = Current.Kind switch
        {
            SyntaxKind.QuotationMarksStringToken => MatchToken(SyntaxKind.QuotationMarksStringToken),
            SyntaxKind.SingleQuotationMarksStringToken => MatchToken(SyntaxKind.SingleQuotationMarksStringToken),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };
#pragma warning restore CA1508 // Avoid dead conditional code

        return new UnknownTableSettingClause(_syntaxTree, identifierToken, colonToken, valueToken);
    }

    private StatementSyntax ParseStatement()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenBraceToken => ParseBlockStatement(),
            SyntaxKind.IndexesKeyword => ParseIndexesDeclaration(),
            SyntaxKind.NoteKeyword => ParseNoteDeclaration(),
            _ when CanReadColumnDeclaration() => ParseColumnDeclaration(),
            _ => ParseExpressionStatement(),
        };
    }

    private bool CanReadColumnDeclaration()
    {
        bool canReadColumnName =
            Current.Kind.IsKeyword() || Current.Kind == SyntaxKind.IdentifierToken;

        static bool CanReadColumnType(SyntaxKind kind)
        {
            return kind == SyntaxKind.IdentifierToken
                || kind.IsKeyword()
                || kind == SyntaxKind.QuotationMarksStringToken
                || kind == SyntaxKind.SingleQuotationMarksStringToken;
        }

        bool canReadColumnType = CanReadColumnType(Lookahead.Kind);
        return canReadColumnName && canReadColumnType;
    }

    private BlockStatementSyntax ParseBlockStatement()
    {
        List<StatementSyntax> statements = new List<StatementSyntax>();

        SyntaxToken openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);

        while (Current.Kind != SyntaxKind.EndOfFileToken &&
               Current.Kind != SyntaxKind.CloseBraceToken)
        {
            SyntaxToken startToken = Current;

            StatementSyntax statement = ParseStatement();
            statements.Add(statement);

            // If ParseStatement() did not consume any tokens,
            // we need to skip the current token and continue
            // in order to avoid an infinite loop.
            //
            // We don't need to report an error, because we'll
            // already tried to parse an expression statement
            // and reported one.
            if (Current == startToken)
                NextToken();
        }

        SyntaxToken closeBraceToken = MatchToken(SyntaxKind.CloseBraceToken);

        return new BlockStatementSyntax(_syntaxTree,
            openBraceToken, statements.ToArray(), closeBraceToken);
    }

    private StatementSyntax ParseIndexesDeclaration()
    {
        SyntaxToken indexesKeyword = MatchToken(SyntaxKind.IndexesKeyword);

        SyntaxToken left = MatchToken(SyntaxKind.OpenBraceToken);

        SeparatedSyntaxList<StatementSyntax> indexes =
            ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseBraceToken,
                separatorKind: null,
                parseExpression: ParseIndexDeclaration);

        SyntaxToken right = MatchToken(SyntaxKind.CloseBraceToken);

        return new IndexesDeclarationSyntax(_syntaxTree, indexesKeyword, left, indexes, right);
    }

    private StatementSyntax ParseIndexDeclaration()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenParenthesisToken => ParseCompositeIndexDeclaration(),
            _ => ParseSingleFieldIndexDeclaration()
        };
    }

    private StatementSyntax ParseCompositeIndexDeclaration()
    {
        ExpressionSyntax ParseCompositeIndexIdentifierExpression()
        {
            return ParseExpression();
        }

        SyntaxToken openParenthesis = MatchToken(SyntaxKind.OpenParenthesisToken);

        SeparatedSyntaxList<ExpressionSyntax> identifiers =
            ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseParenthesisToken,
                separatorKind: SyntaxKind.CommaToken,
                parseExpression: ParseCompositeIndexIdentifierExpression);

        SyntaxToken closeParenthesis = MatchToken(SyntaxKind.CloseParenthesisToken);

        SyntaxToken? openBracket = null;
        SeparatedSyntaxList<ExpressionSyntax>? settings = null;
        SyntaxToken? closeBracket = null;

        if (Current.Kind == SyntaxKind.OpenBracketToken)
        {
            openBracket = MatchToken(SyntaxKind.OpenBracketToken);

            settings = ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseBracketToken,
                separatorKind: SyntaxKind.CommaToken,
                parseExpression: ParseIndexSettingExpression);

            closeBracket = MatchToken(SyntaxKind.CloseBracketToken);
        }

        return new CompositeIndexDeclarationSyntax(_syntaxTree,
            openParenthesis, identifiers, closeParenthesis,
            openBracket, settings, closeBracket);
    }

    private StatementSyntax ParseSingleFieldIndexDeclaration()
    {
        SyntaxToken identifier = MatchToken(SyntaxKind.IdentifierToken);
        IndexSettingListSyntax? settingList = ParseOptionalIndexSettingList();
        return new SingleFieldIndexDeclarationSyntax(_syntaxTree, identifier, settingList);
    }

    private IndexSettingListSyntax? ParseOptionalIndexSettingList()
    {
        return Current.Kind == SyntaxKind.OpenBracketToken
            ? ParseIndexSettingList()
            : null;
    }

    private IndexSettingListSyntax ParseIndexSettingList()
    {
        SyntaxToken openBracketToken = MatchToken(SyntaxKind.OpenBracketToken);

        SeparatedSyntaxList<IndexSettingClause> settings =
            ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseBracketToken,
                separatorKind: SyntaxKind.CommaToken,
                parseExpression: ParseIndexSettingClause);

        SyntaxToken closeBracketToken = MatchToken(SyntaxKind.CloseBracketToken);

        return new IndexSettingListSyntax(_syntaxTree, openBracketToken, settings, closeBracketToken);
    }

    private IndexSettingClause ParseIndexSettingClause()
    {
        switch (Current.Kind)
        {
            case SyntaxKind.UniqueKeyword:
                return ParseUniqueIndexSetting();
            case SyntaxKind.PkKeyword:
                return ParsePkIndexSetting();
            case SyntaxKind.PrimaryKeyword when Lookahead.Kind == SyntaxKind.KeyKeyword:
                return ParsePrimaryKeyIndexSetting();
            case SyntaxKind.NameKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
                return ParseNameIndexSetting();
            case SyntaxKind.TypeKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
                return ParseTypeIndexSetting();
            case SyntaxKind.NoteKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
                return ParseNoteIndexSetting();
            default:
                return ParseUnknownIndexSetting();
        }
    }

    private IndexSettingClause ParseUniqueIndexSetting()
    {
        SyntaxToken uniqueKeyword = MatchToken(SyntaxKind.UniqueKeyword);
        return new UniqueIndexSettingClause(_syntaxTree, uniqueKeyword);
    }

    private IndexSettingClause ParsePkIndexSetting()
    {
        SyntaxToken pkKeyword = MatchToken(SyntaxKind.PkKeyword);
        return new PkIndexSettingClause(_syntaxTree, pkKeyword);
    }

    private IndexSettingClause ParsePrimaryKeyIndexSetting()
    {
        SyntaxToken primaryKeyword = MatchToken(SyntaxKind.PrimaryKeyword);
        SyntaxToken keyKeyword = MatchToken(SyntaxKind.KeyKeyword);
        return new PrimaryKeyIndexSettingClause(_syntaxTree, primaryKeyword, keyKeyword);
    }

    private IndexSettingClause ParseNameIndexSetting()
    {
        SyntaxToken nameKeyword = MatchToken(SyntaxKind.NameKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
        SyntaxKind valueTokenKind = Current.Kind switch
        {
            SyntaxKind.QuotationMarksStringToken => SyntaxKind.QuotationMarksStringToken,
            _ => SyntaxKind.SingleQuotationMarksStringToken,
        };
        SyntaxToken valueToken = MatchToken(valueTokenKind);
        return new NameIndexSettingClause(_syntaxTree, nameKeyword, colonToken, valueToken);
    }

    private IndexSettingClause ParseTypeIndexSetting()
    {
        SyntaxToken typeKeyword = MatchToken(SyntaxKind.TypeKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
        SyntaxKind valueTokenKind = Current.Kind switch
        {
            SyntaxKind.QuotationMarksStringToken => SyntaxKind.QuotationMarksStringToken,
            SyntaxKind.SingleQuotationMarksStringToken => SyntaxKind.SingleQuotationMarksStringToken,
            _ => SyntaxKind.IdentifierToken,
        };
        SyntaxToken valueToken = MatchToken(valueTokenKind);
        return new TypeIndexSettingClause(_syntaxTree, typeKeyword, colonToken, valueToken);
    }

    private IndexSettingClause ParseNoteIndexSetting()
    {
        SyntaxToken noteKeyword = MatchToken(SyntaxKind.NoteKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
        SyntaxKind valueTokenKind = Current.Kind switch
        {
            SyntaxKind.QuotationMarksStringToken => SyntaxKind.QuotationMarksStringToken,
            _ => SyntaxKind.SingleQuotationMarksStringToken,
        };
        SyntaxToken valueToken = MatchToken(valueTokenKind);
        return new NoteIndexSettingClause(_syntaxTree, noteKeyword, colonToken, valueToken);
    }

    private IndexSettingClause ParseUnknownIndexSetting()
    {
        void ReportUnknownIndexSetting(string settingName, int spanStart, int spanEnd)
        {
            SourceText text = _syntaxTree.Text;
            TextSpan span = new TextSpan(spanStart, length: spanEnd - spanStart);
            TextLocation location = new TextLocation(text, span);
            Diagnostics.ReportUnknownIndexSetting(location, settingName);
        }

        SyntaxToken identifierToken = Current.Kind.IsKeyword()
                ? NextToken()
                : MatchToken(SyntaxKind.IdentifierToken);

        if (Current.Kind == SyntaxKind.ColonToken)
        {
            SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);

#pragma warning disable CA1508 // Avoid dead conditional code
            SyntaxToken valueToken = Current.Kind switch
            {
                SyntaxKind.QuotationMarksStringToken => MatchToken(SyntaxKind.QuotationMarksStringToken),
                SyntaxKind.SingleQuotationMarksStringToken => MatchToken(SyntaxKind.SingleQuotationMarksStringToken),
                _ => MatchToken(SyntaxKind.IdentifierToken),
            };
#pragma warning restore CA1508 // Avoid dead conditional code

            ReportUnknownIndexSetting(identifierToken.Text, identifierToken.Start, valueToken.End);
            return new UnknownIndexSettingClause(
                _syntaxTree, identifierToken, colonToken, valueToken);
        }

        ReportUnknownIndexSetting(identifierToken.Text, identifierToken.Start, identifierToken.End);
        return new UnknownIndexSettingClause(_syntaxTree, identifierToken);
    }

    private StatementSyntax ParseColumnDeclaration()
    {
        SyntaxToken identifier = MatchToken(SyntaxKind.IdentifierToken);
        ColumnTypeClause columnTypeClause = ParseColumnTypeClause();
        ColumnSettingListSyntax? settingList = ParseOptionalColumnSettingList();
        return new ColumnDeclarationSyntax(
            _syntaxTree, identifier, columnTypeClause, settingList);
    }

    private ColumnTypeClause ParseColumnTypeClause()
    {
        // String tokens are considered valid column type by themselves.
        if (Current.Kind.IsStringToken())
        {
            SyntaxToken stringColumnTypeToken = NextToken();
            return new ColumnTypeIdentifierClause(_syntaxTree, stringColumnTypeToken);
        }

        SyntaxToken identifierToken =
            Current.Kind.IsKeyword()
                ? NextToken() // Keyword token
                : MatchToken(SyntaxKind.IdentifierToken);

        if (Current.Kind == SyntaxKind.OpenParenthesisToken)
        {
            SyntaxToken openParenthesisToken = MatchToken(SyntaxKind.OpenParenthesisToken);

#pragma warning disable CA1508 // Avoid dead conditional code

            SyntaxKind valueTokenKind =
                Current.Kind == SyntaxKind.NumberToken
                    ? SyntaxKind.NumberToken
                    : Current.Kind.IsKeyword()
                        ? Current.Kind
                        : SyntaxKind.IdentifierToken;

#pragma warning restore CA1508 // Avoid dead conditional code

            SyntaxToken valueToken = MatchToken(valueTokenKind);

            SyntaxToken closeParenthesisToken = MatchToken(SyntaxKind.CloseParenthesisToken);

            return new ColumnTypeParenthesizedIdentifierClause(
                _syntaxTree, identifierToken, openParenthesisToken, valueToken, closeParenthesisToken);
        }

        return new ColumnTypeIdentifierClause(_syntaxTree, identifierToken);
    }

    private ColumnSettingListSyntax? ParseOptionalColumnSettingList()
    {
        return Current.Kind == SyntaxKind.OpenBracketToken
            ? ParseColumnSettingList()
            : null;
    }

    private ColumnSettingListSyntax ParseColumnSettingList()
    {
        SyntaxToken openBracketToken = MatchToken(SyntaxKind.OpenBracketToken);

        SeparatedSyntaxList<ColumnSettingClause> settings =
            ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseBracketToken,
                separatorKind: SyntaxKind.CommaToken,
                parseExpression: ParseColumnSettingClause);

        SyntaxToken closeBracketToken = MatchToken(SyntaxKind.CloseBracketToken);

        return new ColumnSettingListSyntax(_syntaxTree, openBracketToken, settings, closeBracketToken);
    }

    private ColumnSettingClause ParseColumnSettingClause()
    {
        switch (Current.Kind)
        {
            case SyntaxKind.PrimaryKeyword when Lookahead.Kind == SyntaxKind.KeyKeyword:
            {
                SyntaxToken primaryKeyword = MatchToken(SyntaxKind.PrimaryKeyword);
                SyntaxToken keyKeyword = MatchToken(SyntaxKind.KeyKeyword);
                return new PrimaryKeyColumnSettingClause(_syntaxTree, primaryKeyword, keyKeyword);
            }
            case SyntaxKind.PkKeyword:
            {
                SyntaxToken pkKeyword = MatchToken(SyntaxKind.PkKeyword);
                return new PkColumnSettingClause(_syntaxTree, pkKeyword);
            }
            case SyntaxKind.NullKeyword:
            {
                SyntaxToken nullKeyword = MatchToken(SyntaxKind.NullKeyword);
                return new NullColumnSettingClause(_syntaxTree, nullKeyword);
            }
            case SyntaxKind.NotKeyword when Lookahead.Kind == SyntaxKind.NullKeyword:
            {
                SyntaxToken notKeyword = MatchToken(SyntaxKind.NotKeyword);
                SyntaxToken nullKeyword = MatchToken(SyntaxKind.NullKeyword);
                return new NotNullColumnSettingClause(_syntaxTree, notKeyword, nullKeyword);
            }
            case SyntaxKind.UniqueKeyword:
            {
                SyntaxToken uniqueKeyword = MatchToken(SyntaxKind.UniqueKeyword);
                return new UniqueColumnSettingClause(_syntaxTree, uniqueKeyword);
            }
            case SyntaxKind.IncrementKeyword:
            {
                SyntaxToken incrementKeyword = MatchToken(SyntaxKind.IncrementKeyword);
                return new IncrementColumnSettingClause(_syntaxTree, incrementKeyword);
            }
            case SyntaxKind.DefaultKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
            {
                SyntaxToken defaultKeyword = MatchToken(SyntaxKind.DefaultKeyword);
                SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
                SyntaxKind valueTokenKind = Current.Kind switch
                {
                    SyntaxKind.QuotationMarksStringToken => SyntaxKind.QuotationMarksStringToken,
                    SyntaxKind.SingleQuotationMarksStringToken => SyntaxKind.SingleQuotationMarksStringToken,
                    SyntaxKind.NumberToken => SyntaxKind.NumberToken,
                    _ when Current.Kind.IsKeyword() => Current.Kind,
                    _ => SyntaxKind.IdentifierToken
                };
                ExpressionSyntax expressionValue = ParseExpression();
                return new DefaultColumnSettingClause(_syntaxTree, defaultKeyword, colonToken, expressionValue);
            }
            case SyntaxKind.NoteKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
            {
                ReadNoteSettingTokens(
                    out SyntaxToken noteKeyword,
                    out SyntaxToken colonToken,
                    out SyntaxToken noteToken);

                return new NoteColumnSettingClause(_syntaxTree, noteKeyword, colonToken, noteToken);
            }
            case SyntaxKind.RefKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
            {
                ReadRelationshipSettingTokens(
                    out SyntaxToken refKeyword,
                    out SyntaxToken colonToken,
                    out RelationshipConstraintClause constraintClause);

                return new RelationshipColumnSettingClause(_syntaxTree, refKeyword, colonToken, constraintClause);
            }
            default:
            {
                SyntaxToken identifierToken = Current.Kind.IsKeyword()
                        ? NextToken()
                        : MatchToken(SyntaxKind.IdentifierToken);

                if (Current.Kind == SyntaxKind.ColonToken)
                {
                    SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);

#pragma warning disable CA1508 // Avoid dead conditional code
                    SyntaxToken valueToken = Current.Kind switch
                    {
                        SyntaxKind.QuotationMarksStringToken => MatchToken(SyntaxKind.QuotationMarksStringToken),
                        SyntaxKind.SingleQuotationMarksStringToken => MatchToken(SyntaxKind.SingleQuotationMarksStringToken),
                        _ => MatchToken(SyntaxKind.IdentifierToken),
                    };
#pragma warning restore CA1508 // Avoid dead conditional code

                    ReportUnknownColumnSetting(identifierToken.Text, identifierToken.Start, valueToken.End);
                    return new UnknownColumnSettingClause(_syntaxTree, identifierToken, colonToken, valueToken);
                }

                ReportUnknownColumnSetting(identifierToken.Text, identifierToken.Start, identifierToken.End);
                return new UnknownColumnSettingClause(_syntaxTree, identifierToken);
            }
        }

        void ReportUnknownColumnSetting(string settingName, int spanStart, int spanEnd)
        {
            SourceText text = _syntaxTree.Text;
            TextSpan span = new TextSpan(spanStart, length: spanEnd - spanStart);
            TextLocation location = new TextLocation(text, span);
            Diagnostics.ReportUnknownColumnSetting(location, settingName);
        }
    }

    private StatementSyntax ParseNoteDeclaration()
    {
        ReadNoteSettingTokens(
            out SyntaxToken noteKeyword,
            out SyntaxToken colonToken,
            out SyntaxToken noteToken);

        return new NoteDeclarationSyntax(_syntaxTree, noteKeyword, colonToken, noteToken);
    }

    private void ReadRelationshipSettingTokens(
        out SyntaxToken refKeyword,
        out SyntaxToken colonToken,
        out RelationshipConstraintClause constraintClause)
    {
        refKeyword = MatchToken(SyntaxKind.RefKeyword);
        colonToken = MatchToken(SyntaxKind.ColonToken);

        ColumnIdentifierClause? fromIdentifier = ParseOptionalColumnIdentifier();

        SyntaxToken relationshipTypeToken = Current.Kind switch
        {
            // Read one-to-many:  ref: < schema.ToTable.Id
            SyntaxKind.LessToken => MatchToken(SyntaxKind.LessToken),
            // Read many-to-one:  ref: > schema.ToTable.Id
            SyntaxKind.GraterToken => MatchToken(SyntaxKind.GraterToken),
            // Read many-to-many: ref: <> schema.ToTable.Id
            SyntaxKind.LessGraterToken => MatchToken(SyntaxKind.LessGraterToken),
            // Read one-to-one:   ref: - schema.ToTable.Id
            _ => MatchToken(SyntaxKind.MinusToken)
        };

        ColumnIdentifierClause toIdentifier = ParseColumnIdentifier();
        constraintClause =
            new RelationshipConstraintClause(_syntaxTree, fromIdentifier, relationshipTypeToken, toIdentifier);
    }

    private ColumnIdentifierClause? ParseOptionalColumnIdentifier()
    {
        if (Current.Kind == SyntaxKind.IdentifierToken)
            return ParseColumnIdentifier();
        else
            return null;
    }

    private ColumnIdentifierClause ParseColumnIdentifier()
    {
        // Read syntax: column
        SyntaxToken columnIdentifier = MatchToken(SyntaxKind.IdentifierToken);

        // Read syntax: table.column
        SyntaxToken? tableIdentifier = null;
        SyntaxToken? secondDotToken = null;
        if (Current.Kind == SyntaxKind.DotToken)
        {
            tableIdentifier = columnIdentifier;
            secondDotToken = MatchToken(SyntaxKind.DotToken);
            columnIdentifier = MatchToken(SyntaxKind.IdentifierToken);
        }

        // Read syntax: schema.table.column
        SyntaxToken? schemaIdentifier = null;
        SyntaxToken? firstDotToken = null;
        if (Current.Kind == SyntaxKind.DotToken)
        {
            schemaIdentifier = tableIdentifier;
            tableIdentifier = columnIdentifier;
            firstDotToken = MatchToken(SyntaxKind.DotToken);
            columnIdentifier = MatchToken(SyntaxKind.IdentifierToken);
        }

        return new ColumnIdentifierClause(
            _syntaxTree, schemaIdentifier, firstDotToken,
            tableIdentifier, secondDotToken, columnIdentifier);
    }

    private void ReadNoteSettingTokens(
        out SyntaxToken noteKeyword,
        out SyntaxToken colonToken,
        out SyntaxToken noteToken)
    {
        noteKeyword = MatchToken(SyntaxKind.NoteKeyword);
        colonToken = MatchToken(SyntaxKind.ColonToken);
        noteToken = Current.Kind == SyntaxKind.QuotationMarksStringToken
            ? MatchToken(SyntaxKind.QuotationMarksStringToken)
            : MatchToken(SyntaxKind.SingleQuotationMarksStringToken);
    }

    private StatementSyntax ParseExpressionStatement()
    {
        ExpressionSyntax expression = ParseExpression();
        return new ExpressionStatementSyntax(_syntaxTree, expression);
    }

    private ExpressionSyntax ParseExpression()
    {
        return ParsePrimaryExpression();
    }

    private ExpressionSyntax ParsePrimaryExpression()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenParenthesisToken => ParseParenthesizedExpression(),
            SyntaxKind.BacktickToken => ParseBacktickExpression(),
            SyntaxKind.FalseKeyword => ParseBooleanLiteral(),
            SyntaxKind.TrueKeyword => ParseBooleanLiteral(),
            SyntaxKind.NumberToken => ParseNumberLiteral(),
            SyntaxKind.QuotationMarksStringToken => ParseStringLiteral(),
            SyntaxKind.SingleQuotationMarksStringToken => ParseStringLiteral(),
            _ => ParseNameExpression(),
        };
    }

    private ExpressionSyntax ParseParenthesizedExpression()
    {
        SyntaxToken openParenthesisToken = MatchToken(SyntaxKind.OpenParenthesisToken);
        ExpressionSyntax expression = ParseExpression();
        SyntaxToken closeParenthesisToken = MatchToken(SyntaxKind.CloseParenthesisToken);
        return new ParenthesizedExpressionSyntax(_syntaxTree,
            openParenthesisToken, expression, closeParenthesisToken);
    }

    private ExpressionSyntax ParseBacktickExpression()
    {
        SyntaxToken openBacktickToken = MatchToken(SyntaxKind.BacktickToken);
        ExpressionSyntax expression = ParseExpression();
        SyntaxToken closeBacktickToken = MatchToken(SyntaxKind.BacktickToken);
        return new BacktickExpressionSyntax(_syntaxTree,
            openBacktickToken, expression, closeBacktickToken);
    }

    private ExpressionSyntax ParseBooleanLiteral()
    {
        bool isTrue = Current.Kind == SyntaxKind.TrueKeyword;
        SyntaxKind kind = isTrue ? SyntaxKind.TrueKeyword : SyntaxKind.FalseKeyword;

        SyntaxToken keywordToken = MatchToken(kind);
        return new LiteralExpressionSyntax(_syntaxTree, keywordToken, isTrue);
    }

    private ExpressionSyntax ParseNumberLiteral()
    {
        SyntaxToken numberToken = MatchToken(SyntaxKind.NumberToken);
        return new LiteralExpressionSyntax(_syntaxTree, numberToken);
    }

    private ExpressionSyntax ParseStringLiteral()
    {
        SyntaxToken stringToken =
            Current.Kind == SyntaxKind.SingleQuotationMarksStringToken
                ? MatchToken(SyntaxKind.SingleQuotationMarksStringToken)
                : MatchToken(SyntaxKind.QuotationMarksStringToken);

        return new LiteralExpressionSyntax(_syntaxTree, stringToken);
    }

    private ExpressionSyntax ParseNameExpression()
    {
        SyntaxToken identifierToken = MatchToken(SyntaxKind.IdentifierToken);
        return new NameExpressionSyntax(_syntaxTree, identifierToken);
    }

    private ExpressionSyntax ParseIndexSettingExpression()
    {
        SyntaxToken[] identifiers;
        static bool MatchComposedIdentifier(SyntaxToken token) =>
            token.Kind == SyntaxKind.IdentifierToken
            || token.Kind == SyntaxKind.NumberToken
            || token.Kind == SyntaxKind.FalseKeyword
            || token.Kind == SyntaxKind.TrueKeyword;

        // Read index name setting
        if (Current.Kind == SyntaxKind.NameKeyword
            && Lookahead.Kind == SyntaxKind.ColonToken)
        {
            SyntaxToken nameKeyword = MatchToken(SyntaxKind.NameKeyword);
            SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
            SyntaxKind valueTokenKind = Current.Kind switch
            {
                SyntaxKind.QuotationMarksStringToken => SyntaxKind.QuotationMarksStringToken,
                SyntaxKind.SingleQuotationMarksStringToken => SyntaxKind.SingleQuotationMarksStringToken,
                _ => SyntaxKind.IdentifierToken,
            };
            SyntaxToken valueToken = MatchToken(valueTokenKind);

            identifiers = new SyntaxToken[] { nameKeyword, colonToken, valueToken };
        }
        // Read index type setting
        else if (Current.Kind == SyntaxKind.TypeKeyword
            && Lookahead.Kind == SyntaxKind.ColonToken)
        {
            SyntaxToken nameKeyword = MatchToken(SyntaxKind.TypeKeyword);
            SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
            SyntaxKind valueTokenKind = Current.Kind switch
            {
                SyntaxKind.QuotationMarksStringToken => SyntaxKind.QuotationMarksStringToken,
                SyntaxKind.SingleQuotationMarksStringToken => SyntaxKind.SingleQuotationMarksStringToken,
                _ => SyntaxKind.IdentifierToken,
            };
            SyntaxToken valueToken = MatchToken(valueTokenKind);

            identifiers = new SyntaxToken[] { nameKeyword, colonToken, valueToken };
        }
        // Read index composed identifier setting
        else if (MatchComposedIdentifier(Current) && MatchComposedIdentifier(Lookahead))
        {
            identifiers = ParseTokensUntil(condition: () => MatchComposedIdentifier(Current));
        }
        // Read index single identifier setting
        else
        {
            SyntaxToken identifier = NextToken();
            identifiers = new SyntaxToken[] { identifier };
        }

        return new IndexSettingExpressionSyntax(_syntaxTree, identifiers);
    }

    private SyntaxToken[] ParseTokensUntil(Func<bool> condition)
    {
        List<SyntaxToken> identifiers = new List<SyntaxToken>();

        while (condition())
        {
            SyntaxToken identifierToken = NextToken();
            identifiers.Add(identifierToken);
        }

        return identifiers.ToArray();
    }

    private SeparatedSyntaxList<TResult> ParseSeparatedList<TResult>(
        SyntaxKind closeTokenKind,
        SyntaxKind? separatorKind,
        Func<TResult> parseExpression)
        where TResult : SyntaxNode
    {
        ImmutableArray<SyntaxNode>.Builder nodesAndSeparators =
            ImmutableArray.CreateBuilder<SyntaxNode>();

        bool parseNextArgument = true;
        while (parseNextArgument &&
               Current.Kind != closeTokenKind &&
               Current.Kind != SyntaxKind.EndOfFileToken)
        {
            TResult expression = parseExpression();
            nodesAndSeparators.Add(expression);

            if (separatorKind is null)
            {
                SyntaxToken emptySeparator =
                    new SyntaxToken(
                        _syntaxTree, SyntaxKind.BadToken, Current.Start,
                        text: null, value: null);

                nodesAndSeparators.Add(emptySeparator);
            }
            else if (Current.Kind == separatorKind)
            {
                SyntaxToken comma = MatchToken(separatorKind.Value);
                nodesAndSeparators.Add(comma);
            }
            else
            {
                parseNextArgument = false;
            }
        }

        return new SeparatedSyntaxList<TResult>(nodesAndSeparators.ToImmutable());
    }
}
