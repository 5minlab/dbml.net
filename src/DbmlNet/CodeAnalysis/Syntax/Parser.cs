using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis.Syntax;

internal sealed class Parser
{
    internal static readonly string[] IndexSettingTypes = new[] { "btree", "gin", "gist", "hash" };
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

    public DiagnosticBag Diagnostics { get; } = new DiagnosticBag();

    private SyntaxToken Current => Peek(offset: 0);

    private SyntaxToken Lookahead => Peek(offset: 1);

    /// <summary>
    /// Parses the compilation unit and returns the resulting <see cref="CompilationUnitSyntax"/>.
    /// </summary>
    /// <returns>The <see cref="CompilationUnitSyntax"/> object representing the parsed compilation unit.</returns>
    public CompilationUnitSyntax ParseCompilationUnit()
    {
        ImmutableArray<MemberSyntax> members = ParseMembers();

        TableDeclarationSyntax[] tableDeclarations =
            members.OfType<TableDeclarationSyntax>().ToArray();

        HashSet<string> seenTableNames = new(StringComparer.InvariantCulture);
        for (int i = 0; i < tableDeclarations.Length; i++)
        {
            TableDeclarationSyntax tableDeclaration = tableDeclarations[i];
            string tableNameText = tableDeclaration.DbSchema.Text;
            if (!seenTableNames.Add(tableNameText))
            {
                TextSpan columnNameSpan = tableDeclaration.DbSchema.Span;
                TextLocation location = new(_syntaxTree.Text, columnNameSpan);
                Diagnostics.ReportDuplicateTableName(location, tableNameText);
            }
        }

        SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
        return new CompilationUnitSyntax(_syntaxTree, members, endOfFileToken);
    }

    private static void ParseTokens(
        SyntaxTree syntaxTree,
        out ImmutableArray<SyntaxToken> tokens,
        out ImmutableArray<Diagnostic> diagnostics)
    {
        Lexer lexer = new(syntaxTree);

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
                _ => false
            };

            if (skipToken)
                badTokens.Add(token);
            else
                tokenList.Add(token);
        }
        while (token.Kind != SyntaxKind.EndOfFileToken);

        diagnostics = lexer.Diagnostics.ToImmutableArray();
        tokens = tokenList.ToImmutableArray();
    }

    private SyntaxToken Peek(int offset)
    {
        int index = _position + offset;
        return index < _tokens.Length
            ? _tokens[index]
            : _tokens[^1];
    }

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
        return new SyntaxToken(_syntaxTree, kind, Current.Start);
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
            SyntaxKind.EnumKeyword => ParseEnumDeclaration(),
            SyntaxKind.TableKeyword => ParseTableDeclaration(),
            SyntaxKind.RefKeyword => ParseRelationshipDeclaration(),
            _ => ParseGlobalStatement()
        };
    }

    private MemberSyntax ParseProjectDeclaration()
    {
        SyntaxToken projectKeyword = MatchToken(SyntaxKind.ProjectKeyword);
        SyntaxToken identifier = Current.Kind switch
        {
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };

        SyntaxToken openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);
        ProjectSettingListSyntax settings = ParseProjectSettingList();
        SyntaxToken closeBraceToken = MatchToken(SyntaxKind.CloseBraceToken);

        return new ProjectDeclarationSyntax(
            _syntaxTree, projectKeyword, identifier, openBraceToken, settings, closeBraceToken);
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
        return Current.Kind switch
        {
            SyntaxKind.DatabaseTypeKeyword when Lookahead.Kind == SyntaxKind.ColonToken => ParseDatabaseProviderProjectSetting(),
            SyntaxKind.NoteKeyword when Lookahead.Kind == SyntaxKind.ColonToken => ParseNoteProjectSetting(),
            _ => ParseUnknownProjectSetting(),
        };
    }

    private ProjectSettingClause ParseDatabaseProviderProjectSetting()
    {
        SyntaxToken databaseTypeKeyword = MatchToken(SyntaxKind.DatabaseTypeKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);

        SyntaxToken valueToken = Current.Kind switch
        {
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };

        return new DatabaseProviderProjectSettingClause(_syntaxTree, databaseTypeKeyword, colonToken, valueToken);
    }

    private ProjectSettingClause ParseNoteProjectSetting()
    {
        SyntaxToken noteKeyword = MatchToken(SyntaxKind.NoteKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);

        SyntaxToken valueToken = Current.Kind switch
        {
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };

        return new NoteProjectSettingClause(_syntaxTree, noteKeyword, colonToken, valueToken);
    }

    private ProjectSettingClause ParseUnknownProjectSetting()
    {
        void ReportUnknownProjectSetting(string settingName, int spanStart, int spanEnd)
        {
            SourceText text = _syntaxTree.Text;
            TextSpan span = new(spanStart, length: spanEnd - spanStart);
            TextLocation location = new(text, span);
            Diagnostics.ReportUnknownProjectSetting(location, settingName);
        }

        SyntaxToken identifierToken = Current.Kind switch
        {
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken)
        };

        if (Current.Kind == SyntaxKind.ColonToken)
        {
            SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
            SyntaxToken valueToken = Current.Kind switch
            {
                _ when Current.Kind.IsStringToken() => NextToken(),
                _ when Current.Kind.IsKeyword() => NextToken(),
                _ => MatchToken(SyntaxKind.IdentifierToken),
            };

            ReportUnknownProjectSetting(identifierToken.Text, identifierToken.Start, valueToken.End);
            return new UnknownProjectSettingClause(_syntaxTree, identifierToken, colonToken, valueToken);
        }

        ReportUnknownProjectSetting(identifierToken.Text, identifierToken.Start, identifierToken.End);
        return new UnknownProjectSettingClause(_syntaxTree, identifierToken);
    }

    private MemberSyntax ParseEnumDeclaration()
    {
        SyntaxToken enumKeyword = MatchToken(SyntaxKind.EnumKeyword);
        EnumIdentifierClause identifier = ParseEnumIdentifier();
        StatementSyntax body = ParseBlockStatement();

        EnumEntryDeclarationSyntax[] enumEntryDeclarations =
            body.GetChildren().OfType<EnumEntryDeclarationSyntax>().ToArray();

        HashSet<string> seenEnumEntryNames = new(StringComparer.InvariantCulture);
        for (int i = 0; i < enumEntryDeclarations.Length; i++)
        {
            EnumEntryDeclarationSyntax enumEntryDeclaration = enumEntryDeclarations[i];
            string enumEntryNameText = enumEntryDeclaration.IdentifierToken.Text;
            if (!seenEnumEntryNames.Add(enumEntryNameText))
            {
                TextSpan enumEntryNameSpan = enumEntryDeclaration.IdentifierToken.Span;
                TextLocation location = new(_syntaxTree.Text, enumEntryNameSpan);
                Diagnostics.ReportDuplicateEnumEntryName(location, enumEntryNameText);
            }
        }

        return new EnumDeclarationSyntax(_syntaxTree, enumKeyword, identifier, body);
    }

    private EnumIdentifierClause ParseEnumIdentifier()
    {
        // Read syntax: enum
        SyntaxToken enumIdentifier = Current.Kind switch
        {
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken)
        };

        // Read syntax: schema.enum
        SyntaxToken? schemaIdentifier = null;
        SyntaxToken? dotToken = null;
        if (Current.Kind == SyntaxKind.DotToken)
        {
            schemaIdentifier = enumIdentifier;
            dotToken = MatchToken(SyntaxKind.DotToken);
            enumIdentifier = Current.Kind switch
            {
                _ when Current.Kind.IsKeyword() => NextToken(),
                _ => MatchToken(SyntaxKind.IdentifierToken)
            };
        }

        return new EnumIdentifierClause(
            _syntaxTree, schemaIdentifier, dotToken, enumIdentifier);
    }

    private MemberSyntax ParseTableDeclaration()
    {
        SyntaxToken tableKeyword = MatchToken(SyntaxKind.TableKeyword);
        TableIdentifierClause identifier = ParseTableIdentifier();
        TableAliasClause? alias = ParseOptionalTableAlias();
        TableSettingListSyntax? settingList = ParseOptionalTableSettingList();
        StatementSyntax body = ParseBlockStatement();

        ColumnDeclarationSyntax[] columnDeclarations =
            body.GetChildren().OfType<ColumnDeclarationSyntax>().ToArray();

        HashSet<string> seenColumNames = new(StringComparer.InvariantCulture);
        for (int i = 0; i < columnDeclarations.Length; i++)
        {
            ColumnDeclarationSyntax columDeclaration = columnDeclarations[i];
            string columnNameText = columDeclaration.IdentifierToken.Text;
            if (!seenColumNames.Add(columnNameText))
            {
                TextSpan columnNameSpan = columDeclaration.IdentifierToken.Span;
                TextLocation location = new(_syntaxTree.Text, columnNameSpan);
                Diagnostics.ReportDuplicateColumnName(location, columnNameText);
            }
        }

        return new TableDeclarationSyntax(_syntaxTree, tableKeyword, identifier, alias, settingList, body);
    }

    private TableAliasClause? ParseOptionalTableAlias()
    {
        return Current.Kind switch
        {
            SyntaxKind.AsKeyword => ParseTableAlias(),
            _ => null
        };
    }

    private TableAliasClause ParseTableAlias()
    {
        SyntaxToken asKeyword = MatchToken(SyntaxKind.AsKeyword);
        SyntaxToken identifier = MatchToken(SyntaxKind.IdentifierToken);
        return new TableAliasClause(_syntaxTree, asKeyword, identifier);
    }

    private TableIdentifierClause ParseTableIdentifier()
    {
        // Read syntax: table
        SyntaxToken tableIdentifier = Current.Kind switch
        {
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken)
        };

        // Read syntax: schema.table
        SyntaxToken? schemaIdentifier = null;
        SyntaxToken? secondDotToken = null;
        if (Current.Kind == SyntaxKind.DotToken)
        {
            schemaIdentifier = tableIdentifier;
            secondDotToken = MatchToken(SyntaxKind.DotToken);
            tableIdentifier = Current.Kind switch
            {
                _ when Current.Kind.IsKeyword() => NextToken(),
                _ => MatchToken(SyntaxKind.IdentifierToken)
            };
        }

        // Read syntax: database.schema.table
        SyntaxToken? databaseIdentifier = null;
        SyntaxToken? firstDotToken = null;
        if (Current.Kind == SyntaxKind.DotToken)
        {
            databaseIdentifier = schemaIdentifier;
            schemaIdentifier = tableIdentifier;
            firstDotToken = MatchToken(SyntaxKind.DotToken);
            tableIdentifier = Current.Kind switch
            {
                _ when Current.Kind.IsKeyword() => NextToken(),
                _ => MatchToken(SyntaxKind.IdentifierToken)
            };
        }

        return new TableIdentifierClause(
            _syntaxTree, databaseIdentifier, firstDotToken, schemaIdentifier, secondDotToken, tableIdentifier);
    }

    private TableSettingListSyntax? ParseOptionalTableSettingList()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenBracketToken => ParseTableSettingList(),
            _ => null
        };
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
        return Current.Kind switch
        {
            SyntaxKind.HeaderColorKeyword when Lookahead.Kind == SyntaxKind.ColonToken => HeaderColorTableSetting(),
            _ => ParseUnknownTableSetting(),
        };
    }

    private TableSettingClause HeaderColorTableSetting()
    {
        SyntaxToken headerColorKeyword = MatchToken(SyntaxKind.HeaderColorKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
        SyntaxToken valueToken = MatchToken(SyntaxKind.HexTripletToken);
        return new HeaderColorTableSettingClause(
            _syntaxTree, headerColorKeyword, colonToken, valueToken);
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
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };
#pragma warning restore CA1508 // Avoid dead conditional code

        return new UnknownTableSettingClause(_syntaxTree, identifierToken, colonToken, valueToken);
    }

    private RelationshipDeclarationSyntax ParseRelationshipDeclaration()
    {
        SyntaxToken refKeyword = MatchToken(SyntaxKind.RefKeyword);
        SyntaxToken? identifier = Current.Kind switch
        {
            SyntaxKind.IdentifierToken => MatchToken(SyntaxKind.IdentifierToken),
            _ => null
        };

        // Read long form relationship declaration
        if (Current.Kind == SyntaxKind.OpenBraceToken)
        {
            SyntaxToken openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);

            ImmutableArray<RelationshipConstraintClause>.Builder builder =
                ImmutableArray.CreateBuilder<RelationshipConstraintClause>();

            while (Current.Kind is SyntaxKind.IdentifierToken and not SyntaxKind.CloseBraceToken)
            {
                RelationshipConstraintClause relationshipConstraint = ParseRelationshipConstraint();
                builder.Add(relationshipConstraint);
            }

            ImmutableArray<RelationshipConstraintClause> relationships =
                builder.ToImmutable();

            SyntaxToken closeBraceToken = MatchToken(SyntaxKind.CloseBraceToken);
            return new RelationshipLongFormDeclarationSyntax(
                _syntaxTree, refKeyword, identifier, openBraceToken, relationships, closeBraceToken);
        }

        // Read short form relationship declaration
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
        RelationshipConstraintClause relationship = ParseRelationshipConstraint();
        return new RelationshipShortFormDeclarationSyntax(
            _syntaxTree, refKeyword, identifier, colonToken, relationship);
    }

    private RelationshipConstraintClause ParseRelationshipConstraint(bool optionalFrom = false)
    {
        ColumnIdentifierClause? fromIdentifier =
            optionalFrom
                ? ParseOptionalColumnIdentifier()
                : ParseColumnIdentifier();

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

        return new RelationshipConstraintClause(
            _syntaxTree, fromIdentifier, relationshipTypeToken, toIdentifier);
    }

    private MemberSyntax ParseGlobalStatement()
    {
        StatementSyntax statement = ParseStatement();
        return new GlobalStatementSyntax(_syntaxTree, statement);
    }

    private StatementSyntax ParseStatement()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenBraceToken => ParseBlockStatement(),
            SyntaxKind.IndexesKeyword when Lookahead.Kind == SyntaxKind.OpenBraceToken
                => ParseIndexesDeclaration(),
            SyntaxKind.NoteKeyword when Lookahead.Kind == SyntaxKind.ColonToken
                => ParseNoteDeclaration(),
            _ when CanReadEnumEntryDeclaration() => ParseEnumEntryDeclaration(),
            _ when CanReadColumnDeclaration() => ParseColumnDeclaration(),
            _ => ParseExpressionStatement(),
        };
    }

    private bool CanReadColumnDeclaration()
    {
        static bool CanReadColumnName(SyntaxKind kind)
        {
            return kind == SyntaxKind.IdentifierToken
                || kind.IsKeyword()
                || kind.IsStringToken();
        }

        static bool CanReadColumnType(SyntaxKind kind)
        {
            return kind == SyntaxKind.IdentifierToken
                || kind.IsKeyword()
                || kind.IsStringToken();
        }

        return CanReadColumnName(Current.Kind) && CanReadColumnType(Lookahead.Kind);
    }

    private bool CanReadEnumEntryDeclaration()
    {
        bool isAllowedEnumIdentifier = Current.Kind switch
        {
            SyntaxKind.IdentifierToken => true,
            _ when Current.Kind.IsKeyword() => true,
            _ when Current.Kind.IsStringToken() => true,
            _ => false
        };

        bool identifierIsOnSingleLine =
            Current.TrailingTrivia.LastOrDefault()?.Kind == SyntaxKind.LineBreakTrivia;

        bool canReadSettings = Lookahead.Kind == SyntaxKind.OpenBracketToken;

        return isAllowedEnumIdentifier && (identifierIsOnSingleLine || canReadSettings);
    }

    private BlockStatementSyntax ParseBlockStatement()
    {
        List<StatementSyntax> statements = new();

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

        return new BlockStatementSyntax(_syntaxTree, openBraceToken, statements.ToArray(), closeBraceToken);
    }

    private StatementSyntax ParseIndexesDeclaration()
    {
        SyntaxToken indexesKeyword = MatchToken(SyntaxKind.IndexesKeyword);

        SyntaxToken left = MatchToken(SyntaxKind.OpenBraceToken);

        SeparatedSyntaxList<IndexDeclarationStatementSyntax> indexes =
            ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseBraceToken,
                separatorKind: null,
                parseExpression: ParseIndexDeclaration);

        SyntaxToken right = MatchToken(SyntaxKind.CloseBraceToken);

        return new IndexesDeclarationSyntax(_syntaxTree, indexesKeyword, left, indexes, right);
    }

    private IndexDeclarationStatementSyntax ParseIndexDeclaration()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenParenthesisToken => ParseCompositeIndexDeclaration(),
            _ => ParseSingleFieldIndexDeclaration()
        };
    }

    private IndexDeclarationStatementSyntax ParseCompositeIndexDeclaration()
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

        IndexSettingListSyntax? settingsList = ParseOptionalIndexSettingList();

        return new CompositeIndexDeclarationSyntax(
            _syntaxTree, openParenthesis, identifiers, closeParenthesis, settingsList);
    }

    private IndexDeclarationStatementSyntax ParseSingleFieldIndexDeclaration()
    {
        SyntaxToken identifier = Current.Kind switch
        {
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken)
        };
        IndexSettingListSyntax? settingsList = ParseOptionalIndexSettingList();

        return new SingleFieldIndexDeclarationSyntax(_syntaxTree, identifier, settingsList);
    }

    private IndexSettingListSyntax? ParseOptionalIndexSettingList()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenBracketToken => ParseIndexSettingList(),
            _ => null
        };
    }

    private IndexSettingListSyntax ParseIndexSettingList()
    {
        SyntaxToken openBracketToken = MatchToken(SyntaxKind.OpenBracketToken);

        SeparatedSyntaxList<IndexSettingClause> settings =
            ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseBracketToken,
                separatorKind: SyntaxKind.CommaToken,
                parseExpression: ParseIndexSettingClause);

        HashSet<string> seenSettingNames = new(StringComparer.InvariantCulture);
        foreach (IndexSettingClause indexSetting in settings)
        {
            string settingNameText = indexSetting.SettingName;
            if (!seenSettingNames.Add(settingNameText))
            {
                TextSpan settingNameSpan = indexSetting.Span;
                TextLocation location = new(_syntaxTree.Text, settingNameSpan);
                Diagnostics.ReportDuplicateIndexSettingName(location, settingNameText);
            }
        }

        SyntaxToken closeBracketToken = MatchToken(SyntaxKind.CloseBracketToken);

        return new IndexSettingListSyntax(_syntaxTree, openBracketToken, settings, closeBracketToken);
    }

    private IndexSettingClause ParseIndexSettingClause()
    {
        return Current.Kind switch
        {
            SyntaxKind.UniqueKeyword => ParseUniqueIndexSetting(),
            SyntaxKind.PkKeyword => ParsePkIndexSetting(),
            SyntaxKind.PrimaryKeyword when Lookahead.Kind == SyntaxKind.KeyKeyword => ParsePrimaryKeyIndexSetting(),
            SyntaxKind.NameKeyword when Lookahead.Kind == SyntaxKind.ColonToken => ParseNameIndexSetting(),
            SyntaxKind.TypeKeyword when Lookahead.Kind == SyntaxKind.ColonToken => ParseTypeIndexSetting(),
            SyntaxKind.NoteKeyword when Lookahead.Kind == SyntaxKind.ColonToken => ParseNoteIndexSetting(),
            _ => ParseUnknownIndexSetting(),
        };
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
        SyntaxToken valueToken = Current.Kind switch
        {
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };
        return new NameIndexSettingClause(_syntaxTree, nameKeyword, colonToken, valueToken);
    }

    private IndexSettingClause ParseTypeIndexSetting()
    {
        SyntaxToken typeKeyword = MatchToken(SyntaxKind.TypeKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
        SyntaxToken valueToken = Current.Kind switch
        {
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };

        string columnTypeName = $"{valueToken.Value ?? valueToken.Text}";
        if (!IndexSettingTypes.Contains(columnTypeName, StringComparer.InvariantCulture))
        {
            TextLocation location = new(_syntaxTree.Text, valueToken.Span);
            Diagnostics.ReportUnknownIndexSettingType(location, columnTypeName);
        }

        return new TypeIndexSettingClause(_syntaxTree, typeKeyword, colonToken, valueToken);
    }

    private IndexSettingClause ParseNoteIndexSetting()
    {
        SyntaxToken noteKeyword = MatchToken(SyntaxKind.NoteKeyword);
        SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
        SyntaxToken valueToken = Current.Kind switch
        {
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken),
        };
        return new NoteIndexSettingClause(_syntaxTree, noteKeyword, colonToken, valueToken);
    }

    private IndexSettingClause ParseUnknownIndexSetting()
    {
        void ReportUnknownIndexSetting(string settingName, int spanStart, int spanEnd)
        {
            SourceText text = _syntaxTree.Text;
            TextSpan span = new(spanStart, length: spanEnd - spanStart);
            TextLocation location = new(text, span);
            Diagnostics.ReportUnknownIndexSetting(location, settingName);
        }

        SyntaxToken identifierToken = Current.Kind.IsKeyword()
                ? NextToken()
                : MatchToken(SyntaxKind.IdentifierToken);

        if (Current.Kind == SyntaxKind.ColonToken)
        {
            SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
            SyntaxToken valueToken = Current.Kind switch
            {
                _ when Current.Kind.IsStringToken() => NextToken(),
                _ => MatchToken(SyntaxKind.IdentifierToken),
            };

            ReportUnknownIndexSetting(identifierToken.Text, identifierToken.Start, valueToken.End);
            return new UnknownIndexSettingClause(
                _syntaxTree, identifierToken, colonToken, valueToken);
        }

        ReportUnknownIndexSetting(identifierToken.Text, identifierToken.Start, identifierToken.End);
        return new UnknownIndexSettingClause(_syntaxTree, identifierToken);
    }

    private StatementSyntax ParseEnumEntryDeclaration()
    {
        SyntaxToken identifier = Current.Kind switch
        {
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken)
        };

        EnumEntrySettingListSyntax? settingList = ParseOptionalEnumEntrySettingList();
        SeparatedSyntaxList<EnumEntrySettingClause> settings =
            settingList?.Settings ?? SeparatedSyntaxList<EnumEntrySettingClause>.Empty;

        HashSet<string> seenSettingNames = new(StringComparer.InvariantCulture);
        foreach (EnumEntrySettingClause columnSetting in settings)
        {
            string settingNameText = columnSetting.SettingName;
            if (!seenSettingNames.Add(settingNameText))
            {
                TextSpan settingNameSpan = columnSetting.Span;
                TextLocation location = new(_syntaxTree.Text, settingNameSpan);
                Diagnostics.ReportDuplicateEnumEntrySettingName(location, settingNameText);
            }
        }

        return new EnumEntryDeclarationSyntax(_syntaxTree, identifier, settingList);
    }

    private EnumEntrySettingListSyntax? ParseOptionalEnumEntrySettingList()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenBracketToken => ParseEnumEntrySettingList(),
            _ => null
        };
    }

    private EnumEntrySettingListSyntax ParseEnumEntrySettingList()
    {
        SyntaxToken openBracketToken = MatchToken(SyntaxKind.OpenBracketToken);

        SeparatedSyntaxList<EnumEntrySettingClause> settings =
            ParseSeparatedList(
                closeTokenKind: SyntaxKind.CloseBracketToken,
                separatorKind: SyntaxKind.CommaToken,
                parseExpression: ParseEnumEntrySettingClause);

        SyntaxToken closeBracketToken = MatchToken(SyntaxKind.CloseBracketToken);

        return new EnumEntrySettingListSyntax(_syntaxTree, openBracketToken, settings, closeBracketToken);
    }

    private EnumEntrySettingClause ParseEnumEntrySettingClause()
    {
        switch (Current.Kind)
        {
            case SyntaxKind.NoteKeyword when Lookahead.Kind == SyntaxKind.ColonToken:
            {
                ReadNoteSettingTokens(
                    out SyntaxToken noteKeyword,
                    out SyntaxToken colonToken,
                    out SyntaxToken noteToken);

                return new NoteEnumEntrySettingClause(_syntaxTree, noteKeyword, colonToken, noteToken);
            }

            default:
            {
                SyntaxToken identifierToken = Current.Kind switch
                {
                    _ when Current.Kind.IsKeyword() => NextToken(),
                    _ => MatchToken(SyntaxKind.IdentifierToken)
                };

                if (Current.Kind == SyntaxKind.ColonToken)
                {
                    SyntaxToken colonToken = MatchToken(SyntaxKind.ColonToken);
                    SyntaxToken valueToken = Current.Kind switch
                    {
                        _ when Current.Kind.IsStringToken() => NextToken(),
                        _ => MatchToken(SyntaxKind.IdentifierToken),
                    };

                    ReportUnknownEnumEntrySetting(identifierToken.Text, identifierToken.Start, valueToken.End);
                    return new UnknownEnumEntrySettingClause(_syntaxTree, identifierToken, colonToken, valueToken);
                }

                ReportUnknownEnumEntrySetting(identifierToken.Text, identifierToken.Start, identifierToken.End);
                return new UnknownEnumEntrySettingClause(_syntaxTree, identifierToken);
            }
        }

        void ReportUnknownEnumEntrySetting(string settingName, int spanStart, int spanEnd)
        {
            SourceText text = _syntaxTree.Text;
            TextSpan span = new(spanStart, length: spanEnd - spanStart);
            TextLocation location = new(text, span);
            Diagnostics.ReportUnknownEnumEntrySetting(location, settingName);
        }
    }

    private StatementSyntax ParseColumnDeclaration()
    {
        SyntaxToken identifier = Current.Kind switch
        {
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ when Current.Kind.IsStringToken() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken)
        };

        ColumnTypeClause columnTypeClause = ParseColumnTypeClause();
        ColumnSettingListSyntax? settingList = ParseOptionalColumnSettingList();
        SeparatedSyntaxList<ColumnSettingClause> settings =
            settingList?.Settings ?? SeparatedSyntaxList<ColumnSettingClause>.Empty;

        HashSet<string> seenSettingNames = new(StringComparer.InvariantCulture);
        foreach (ColumnSettingClause columnSetting in settings)
        {
            string settingNameText = columnSetting.SettingName;
            if (!seenSettingNames.Add(settingNameText))
            {
                TextSpan settingNameSpan = columnSetting.Span;
                TextLocation location = new(_syntaxTree.Text, settingNameSpan);
                Diagnostics.ReportDuplicateColumnSettingName(location, settingNameText);
            }
        }

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

            SyntaxToken valueToken = Current.Kind switch
            {
                SyntaxKind.NumberToken => NextToken(),
                _ when Current.Kind.IsKeyword() => NextToken(),
                _ => MatchToken(SyntaxKind.IdentifierToken),
            };

#pragma warning restore CA1508 // Avoid dead conditional code

            SyntaxToken closeParenthesisToken = MatchToken(SyntaxKind.CloseParenthesisToken);

            return new ColumnTypeParenthesizedIdentifierClause(
                _syntaxTree, identifierToken, openParenthesisToken, valueToken, closeParenthesisToken);
        }

        return new ColumnTypeIdentifierClause(_syntaxTree, identifierToken);
    }

    private ColumnSettingListSyntax? ParseOptionalColumnSettingList()
    {
        return Current.Kind switch
        {
            SyntaxKind.OpenBracketToken => ParseColumnSettingList(),
            _ => null
        };
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

#pragma warning disable MA0051 // Method is too long (maximum allowed: 60)

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

                ExpressionSyntax expressionValue = ParseExpression();
                switch (expressionValue.Kind)
                {
                    case SyntaxKind.LiteralExpression:
                    case SyntaxKind.BacktickExpression:
                    case SyntaxKind.NullExpression:
                    case SyntaxKind.ParenthesizedExpression:
                    case SyntaxKind.CallExpression:
                        // Allow expression
                        break;
                    default:
                        // Disallow expression
                        Diagnostics.ReportDisallowedColumnSettingDefaultValue(expressionValue.Location, expressionValue.Kind);
                        break;
                }

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
                    SyntaxToken valueToken = Current.Kind switch
                    {
                        _ when Current.Kind.IsStringToken() => NextToken(),
                        _ => MatchToken(SyntaxKind.IdentifierToken),
                    };

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
            TextSpan span = new(spanStart, length: spanEnd - spanStart);
            TextLocation location = new(text, span);
            Diagnostics.ReportUnknownColumnSetting(location, settingName);
        }
    }

#pragma warning restore MA0051 // Method is too long (maximum allowed: 60)

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
        constraintClause = ParseRelationshipConstraint(optionalFrom: true);
    }

    private ColumnIdentifierClause? ParseOptionalColumnIdentifier()
    {
        return Current.Kind switch
        {
            SyntaxKind.IdentifierToken => ParseColumnIdentifier(),
            _ when Current.Kind.IsKeyword() => ParseColumnIdentifier(),
            _ => null
        };
    }

    private ColumnIdentifierClause ParseColumnIdentifier()
    {
        // Read syntax: column
        SyntaxToken columnIdentifier = Current.Kind switch
        {
            _ when Current.Kind.IsKeyword() => NextToken(),
            _ => MatchToken(SyntaxKind.IdentifierToken)
        };

        // Read syntax: table.column
        SyntaxToken? tableIdentifier = null;
        SyntaxToken? secondDotToken = null;
        if (Current.Kind == SyntaxKind.DotToken)
        {
            tableIdentifier = columnIdentifier;
            secondDotToken = MatchToken(SyntaxKind.DotToken);
            columnIdentifier = Current.Kind switch
            {
                _ when Current.Kind.IsKeyword() => NextToken(),
                _ => MatchToken(SyntaxKind.IdentifierToken)
            };
        }

        // Read syntax: schema.table.column
        SyntaxToken? schemaIdentifier = null;
        SyntaxToken? firstDotToken = null;
        if (Current.Kind == SyntaxKind.DotToken)
        {
            schemaIdentifier = tableIdentifier;
            tableIdentifier = columnIdentifier;
            firstDotToken = MatchToken(SyntaxKind.DotToken);
            columnIdentifier = Current.Kind switch
            {
                _ when Current.Kind.IsKeyword() => NextToken(),
                _ => MatchToken(SyntaxKind.IdentifierToken)
            };
        }

        return new ColumnIdentifierClause(
            _syntaxTree, schemaIdentifier, firstDotToken, tableIdentifier, secondDotToken, columnIdentifier);
    }

    private void ReadNoteSettingTokens(
        out SyntaxToken noteKeyword,
        out SyntaxToken colonToken,
        out SyntaxToken noteToken)
    {
        noteKeyword = MatchToken(SyntaxKind.NoteKeyword);
        colonToken = MatchToken(SyntaxKind.ColonToken);
        noteToken = Current.Kind switch
        {
            SyntaxKind.MultiLineStringToken => MatchToken(SyntaxKind.MultiLineStringToken),
            SyntaxKind.QuotationMarksStringToken => MatchToken(SyntaxKind.QuotationMarksStringToken),
            _ => MatchToken(SyntaxKind.SingleQuotationMarksStringToken),
        };
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
            SyntaxKind.NullKeyword => ParseNullExpression(),
            SyntaxKind.FalseKeyword => ParseBooleanLiteral(),
            SyntaxKind.TrueKeyword => ParseBooleanLiteral(),
            SyntaxKind.NumberToken => ParseNumberLiteral(),
            SyntaxKind.QuotationMarksStringToken => ParseStringLiteral(),
            SyntaxKind.SingleQuotationMarksStringToken => ParseStringLiteral(),
            _ => ParseNameOrCallExpression(),
        };
    }

    private ExpressionSyntax ParseParenthesizedExpression()
    {
        SyntaxToken openParenthesisToken = MatchToken(SyntaxKind.OpenParenthesisToken);
        ExpressionSyntax expression = ParseExpression();
        SyntaxToken closeParenthesisToken = MatchToken(SyntaxKind.CloseParenthesisToken);
        return new ParenthesizedExpressionSyntax(
            _syntaxTree, openParenthesisToken, expression, closeParenthesisToken);
    }

    private ExpressionSyntax ParseBacktickExpression()
    {
        SyntaxToken openBacktickToken = MatchToken(SyntaxKind.BacktickToken);
        ExpressionSyntax expression = ParseExpression();
        SyntaxToken closeBacktickToken = MatchToken(SyntaxKind.BacktickToken);
        return new BacktickExpressionSyntax(
            _syntaxTree, openBacktickToken, expression, closeBacktickToken);
    }

    private ExpressionSyntax ParseNullExpression()
    {
        SyntaxToken keywordToken = MatchToken(SyntaxKind.NullKeyword);
        return new NullExpressionSyntax(_syntaxTree, keywordToken);
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

    private ExpressionSyntax ParseNameOrCallExpression()
    {
        bool canReadCallExpression =
            Current.Kind == SyntaxKind.IdentifierToken
            && Lookahead.Kind == SyntaxKind.OpenParenthesisToken;

        return canReadCallExpression
            ? ParseCallExpression()
            : ParseNameExpression();
    }

    private ExpressionSyntax ParseCallExpression()
    {
        SyntaxToken identifier = MatchToken(SyntaxKind.IdentifierToken);
        SyntaxToken openParenthesisToken = MatchToken(SyntaxKind.OpenParenthesisToken);
        SeparatedSyntaxList<ExpressionSyntax> arguments = ParseArguments();
        SyntaxToken closeParenthesisToken = MatchToken(SyntaxKind.CloseParenthesisToken);
        return new CallExpressionSyntax(_syntaxTree, identifier, openParenthesisToken, arguments, closeParenthesisToken);
    }

    private SeparatedSyntaxList<ExpressionSyntax> ParseArguments()
    {
        ImmutableArray<SyntaxNode>.Builder nodesAndSeparators = ImmutableArray.CreateBuilder<SyntaxNode>();

        bool parseNextArgument = true;
        while (parseNextArgument &&
               Current.Kind != SyntaxKind.CloseParenthesisToken &&
               Current.Kind != SyntaxKind.EndOfFileToken)
        {
            ExpressionSyntax expression = ParseExpression();
            nodesAndSeparators.Add(expression);

            if (Current.Kind == SyntaxKind.CommaToken)
            {
                SyntaxToken comma = MatchToken(SyntaxKind.CommaToken);
                nodesAndSeparators.Add(comma);
            }
            else
            {
                parseNextArgument = false;
            }
        }

        return new SeparatedSyntaxList<ExpressionSyntax>(nodesAndSeparators.ToImmutable());
    }

    private ExpressionSyntax ParseNameExpression()
    {
        SyntaxToken identifierToken = MatchToken(SyntaxKind.IdentifierToken);
        return new NameExpressionSyntax(_syntaxTree, identifierToken);
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
                    new(_syntaxTree, SyntaxKind.BadToken, Current.Start);

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
