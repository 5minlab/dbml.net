using System;
using System.Data;

namespace DbmlNet.CodeAnalysis.Syntax;

/// <summary>
/// </summary>
public abstract class SyntaxWalker
{
    /// <summary>
    /// </summary>
    /// <param name="syntaxTree"></param>
    protected virtual void Walk(SyntaxTree syntaxTree)
    {
        ArgumentNullException.ThrowIfNull(syntaxTree);
        WalkCompilationUnit(syntaxTree.Root);
    }

    /// <summary>
    /// </summary>
    /// <param name="compilationUnit"></param>
    protected virtual void WalkCompilationUnit(CompilationUnitSyntax compilationUnit)
    {
        ArgumentNullException.ThrowIfNull(compilationUnit);
        foreach (MemberSyntax syntax in compilationUnit.Members)
            WalkMember(syntax);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    /// <exception cref="EvaluateException"></exception>
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
            case SyntaxKind.GlobalStatementMember:
                WalkGlobalStatement((GlobalStatementSyntax)syntax);
                break;
            default:
                throw new EvaluateException($"ERROR: Unknown syntax kind <{syntax.Kind}>.");
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkProjectDeclaration(ProjectDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        // TODO: WalkStatement(syntax.Settings);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkTableDeclaration(TableDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        WalkStatement(syntax.Body);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkGlobalStatement(GlobalStatementSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        WalkStatement(syntax.Statement);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    /// <exception cref="EvaluateException"></exception>
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
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkBlockStatement(BlockStatementSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        foreach (StatementSyntax statement in syntax.Statements)
            WalkStatement(statement);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkColumnDeclarationStatement(ColumnDeclarationSyntax syntax)
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkSingleFieldIndexDeclarationStatement(SingleFieldIndexDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        if (syntax.Settings is null)
            return;

        foreach (ExpressionSyntax setting in syntax.Settings)
            WalkIndexSettingExpression((IndexSettingExpressionSyntax)setting);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    private void WalkCompositeIndexDeclarationStatement(CompositeIndexDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        foreach (ExpressionSyntax identifier in syntax.Identifiers)
            WalkExpression(identifier);

        if (syntax.Settings is null)
            return;

        foreach (ExpressionSyntax setting in syntax.Settings)
            WalkIndexSettingExpression((IndexSettingExpressionSyntax)setting);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkIndexesDeclarationStatement(IndexesDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        if (syntax.Indexes.Count <= 0)
            return;

        foreach (StatementSyntax statement in syntax.Indexes)
            WalkStatement(statement);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkNoteDeclarationStatement(NoteDeclarationSyntax syntax)
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    /// <exception cref="EvaluateException"></exception>
    protected virtual void WalkExpressionStatement(ExpressionStatementSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        WalkExpression(syntax.Expression);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkExpression(ExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        switch (syntax.Kind)
        {
            case SyntaxKind.LiteralExpression:
                WalkLiteralExpression((LiteralExpressionSyntax)syntax);
                break;
            case SyntaxKind.NameExpression:
                WalkNameExpression((NameExpressionSyntax)syntax);
                break;
            case SyntaxKind.IndexSettingExpression:
                WalkIndexSettingExpression((IndexSettingExpressionSyntax)syntax);
                break;
            case SyntaxKind.ParenthesizedExpression:
                WalkParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);
                break;
            default:
                throw new EvaluateException($"ERROR: Unknown syntax kind <{syntax.Kind}>.");
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkLiteralExpression(LiteralExpressionSyntax syntax)
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkNameExpression(NameExpressionSyntax syntax)
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkIndexSettingExpression(IndexSettingExpressionSyntax syntax)
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="syntax"></param>
    protected virtual void WalkParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);
        WalkExpression(syntax.Expression);
    }
}
