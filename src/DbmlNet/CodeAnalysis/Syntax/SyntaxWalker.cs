using System;
using System.Data;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// Represents a syntax walker.
/// </summary>
public abstract class SyntaxWalker
{
    /// <summary>
    /// Performs a syntax tree walk starting from the specified syntax tree.
    /// </summary>
    /// <param name="syntaxTree">The syntax tree to walk.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntaxTree"/> is <see langword="null"/>.</exception>
    protected virtual void Walk(SyntaxTree syntaxTree)
    {
        ArgumentNullException.ThrowIfNull(syntaxTree);
        WalkCompilationUnit(syntaxTree.Root);
    }

    /// <summary>
    /// Walks the compilation unit syntax.
    /// </summary>
    /// <param name="compilationUnit">The compilation unit syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="compilationUnit"/> is <see langword="null"/>.</exception>
    protected virtual void WalkCompilationUnit(CompilationUnitSyntax compilationUnit)
    {
        ArgumentNullException.ThrowIfNull(compilationUnit);
        foreach (MemberSyntax syntax in compilationUnit.Members)
            WalkMember(syntax);
    }

    /// <summary>
    /// Walks a member syntax.
    /// </summary>
    /// <param name="syntax">The member syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    /// <exception cref="EvaluateException"><paramref name="syntax.Kind"/> is unknown.</exception>
    protected virtual void WalkMember(MemberSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        switch (syntax.Kind)
        {
            case SyntaxKind.ProjectDeclarationMember:
                WalkProjectDeclaration((ProjectDeclarationSyntax)syntax);
                break;
            case SyntaxKind.TableDeclarationMember:
                WalkTableDeclaration((TableDeclarationSyntax)syntax);
                break;
            case SyntaxKind.RelationshipShortFormDeclarationMember:
                WalkRelationshipShortFormDeclaration((RelationshipShortFormDeclarationSyntax)syntax);
                break;
            case SyntaxKind.RelationshipLongFormDeclarationMember:
                WalkRelationshipLongFormDeclaration((RelationshipLongFormDeclarationSyntax)syntax);
                break;
            case SyntaxKind.GlobalStatementMember:
                WalkGlobalStatement((GlobalStatementSyntax)syntax);
                break;
            default:
                throw new EvaluateException($"ERROR: Unknown syntax kind <{syntax.Kind}>.");
        }
    }

    /// <summary>
    /// Walks a project declaration syntax.
    /// </summary>
    /// <param name="syntax">The project declaration syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkProjectDeclaration(ProjectDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a table declaration syntax.
    /// </summary>
    /// <param name="syntax">The table declaration syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkTableDeclaration(TableDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        WalkStatement(syntax.Body);
    }

    /// <summary>
    /// Walks a short form relationship declaration syntax.
    /// </summary>
    /// <param name="syntax">The short form relationship declaration syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkRelationshipShortFormDeclaration(RelationshipShortFormDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a long form relationship declaration syntax.
    /// </summary>
    /// <param name="syntax">The long form relationship declaration syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkRelationshipLongFormDeclaration(RelationshipLongFormDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a global statement syntax.
    /// </summary>
    /// <param name="syntax">The global statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkGlobalStatement(GlobalStatementSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        WalkStatement(syntax.Statement);
    }

    /// <summary>
    /// Walks a statement syntax.
    /// </summary>
    /// <param name="syntax">The statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    /// <exception cref="EvaluateException"><paramref name="syntax.Kind"/> is unknown.</exception>
    protected virtual void WalkStatement(StatementSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        switch (syntax.Kind)
        {
            case SyntaxKind.BlockStatement:
                WalkBlockStatement((BlockStatementSyntax)syntax);
                break;
            case SyntaxKind.ColumnDeclarationStatement:
                WalkColumnDeclarationStatement((ColumnDeclarationSyntax)syntax);
                break;
            case SyntaxKind.SingleFieldIndexDeclarationStatement:
                WalkSingleFieldIndexDeclarationStatement((SingleFieldIndexDeclarationSyntax)syntax);
                break;
            case SyntaxKind.CompositeIndexDeclarationStatement:
                WalkCompositeIndexDeclarationStatement((CompositeIndexDeclarationSyntax)syntax);
                break;
            case SyntaxKind.IndexesDeclarationStatement:
                WalkIndexesDeclarationStatement((IndexesDeclarationSyntax)syntax);
                break;
            case SyntaxKind.NoteDeclarationStatement:
                WalkNoteDeclarationStatement((NoteDeclarationSyntax)syntax);
                break;
            case SyntaxKind.ExpressionStatement:
                WalkExpressionStatement((ExpressionStatementSyntax)syntax);
                break;
            default:
                throw new EvaluateException($"ERROR: Unknown syntax kind <{syntax.Kind}>.");
        }
    }

    /// <summary>
    /// Walks a block statement syntax.
    /// </summary>
    /// <param name="syntax">The block statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkBlockStatement(BlockStatementSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        foreach (StatementSyntax statement in syntax.Statements)
            WalkStatement(statement);
    }

    /// <summary>
    /// Walks a column declaration statement syntax.
    /// </summary>
    /// <param name="syntax">The column declaration statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkColumnDeclarationStatement(ColumnDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a single field index declaration statement syntax.
    /// </summary>
    /// <param name="syntax">The single field index declaration statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkSingleFieldIndexDeclarationStatement(SingleFieldIndexDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a composite index declaration statement syntax.
    /// </summary>
    /// <param name="syntax">The composite index declaration statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkCompositeIndexDeclarationStatement(CompositeIndexDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        foreach (ExpressionSyntax identifier in syntax.Identifiers)
            WalkExpression(identifier);
    }

    /// <summary>
    /// Walks an indexes declaration statement syntax.
    /// </summary>
    /// <param name="syntax">The indexes declaration statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkIndexesDeclarationStatement(IndexesDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        if (syntax.Indexes.Count <= 0)
            return;

        foreach (StatementSyntax statement in syntax.Indexes)
            WalkStatement(statement);
    }

    /// <summary>
    /// Walks a note declaration statement syntax.
    /// </summary>
    /// <param name="syntax">The note declaration statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkNoteDeclarationStatement(NoteDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks an expression statement syntax.
    /// </summary>
    /// <param name="syntax">The expression statement syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkExpressionStatement(ExpressionStatementSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        WalkExpression(syntax.Expression);
    }

    /// <summary>
    /// Walks an expression syntax.
    /// </summary>
    /// <param name="syntax">The expression syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    /// <exception cref="EvaluateException"><paramref name="syntax.Kind"/> is unknown.</exception>
    protected virtual void WalkExpression(ExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        switch (syntax.Kind)
        {
            case SyntaxKind.BacktickExpression:
                WalkBacktickExpression((BacktickExpressionSyntax)syntax);
                break;
            case SyntaxKind.NullExpression:
                WalkNullExpression((NullExpressionSyntax)syntax);
                break;
            case SyntaxKind.LiteralExpression:
                WalkLiteralExpression((LiteralExpressionSyntax)syntax);
                break;
            case SyntaxKind.NameExpression:
                WalkNameExpression((NameExpressionSyntax)syntax);
                break;
            case SyntaxKind.ParenthesizedExpression:
                WalkParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);
                break;
            default:
                throw new EvaluateException($"ERROR: Unknown syntax kind <{syntax.Kind}>.");
        }
    }

    /// <summary>
    /// Walks a backtick expression syntax.
    /// </summary>
    /// <param name="syntax">The backtick expression syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkBacktickExpression(BacktickExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a null expression syntax.
    /// </summary>
    /// <param name="syntax">The null expression syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkNullExpression(NullExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a literal expression syntax.
    /// </summary>
    /// <param name="syntax">The literal expression syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkLiteralExpression(LiteralExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a name expression syntax.
    /// </summary>
    /// <param name="syntax">The name expression syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkNameExpression(NameExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
    }

    /// <summary>
    /// Walks a parenthesized expression syntax.
    /// </summary>
    /// <param name="syntax">The parenthesized expression syntax.</param>
    /// <exception cref="ArgumentNullException"><paramref name="syntax"/> is <see langword="null"/>.</exception>
    protected virtual void WalkParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        WalkExpression(syntax.Expression);
    }
}
