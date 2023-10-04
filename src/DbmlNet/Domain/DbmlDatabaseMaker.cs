using System;
using System.Data;
using System.Linq;

using DbmlNet.CodeAnalysis.Syntax;

namespace DbmlNet.Domain;

internal sealed class DbmlDatabaseMaker : SyntaxWalker
{
    private readonly DbmlDatabase _database;
    private DbmlProject? _currentProject;
    private DbmlTable? _currentTable;
    private DbmlTableColumn? _currentTableColumn;
    private DbmlTableIndex? _currentTableIndex;

    private DbmlDatabaseMaker(SyntaxTree syntaxTree)
    {
        _database = new DbmlDatabase();
        Walk(syntaxTree);
    }

    /// <summary>
    /// </summary>
    /// <param name="syntaxTree"></param>
    /// <returns></returns>
    public static DbmlDatabase Make(SyntaxTree syntaxTree)
    {
        DbmlDatabaseMaker maker = new DbmlDatabaseMaker(syntaxTree);
        return maker._database;
    }

    /// <inheritdoc/>
    protected override void Walk(SyntaxTree syntaxTree)
    {
        base.Walk(syntaxTree);
    }

    /// <inheritdoc/>
    protected override void WalkProjectDeclaration(ProjectDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        string projectName = $"{syntax.IdentifierToken.Value ?? syntax.IdentifierToken.Text}";
        _currentProject = new DbmlProject(projectName);

        foreach (ProjectSettingClause setting in syntax.Settings.Settings)
        {
            if (setting.Kind == SyntaxKind.DatabaseProviderProjectSettingClause)
            {
                DatabaseProviderProjectSettingClause databaseProviderSetting =
                    (DatabaseProviderProjectSettingClause)setting;

                string providerName =
                    $"{databaseProviderSetting.ValueToken.Value ?? databaseProviderSetting.ValueToken.Text}";

                _database.AddProvider(providerName);
            }
            else if (setting.Kind == SyntaxKind.NoteProjectSettingClause)
            {
                NoteProjectSettingClause noteSetting = (NoteProjectSettingClause)setting;
                string note = noteSetting.ValueToken.Text;
                _database.AddNote(note);
            }
        }

        _database.Project = _currentProject;
        _currentProject = null;
    }

    /// <inheritdoc/>
    protected override void WalkTableDeclaration(TableDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        string tableName = $"{syntax.IdentifierToken.Value ?? syntax.IdentifierToken.Text}";
        _currentTable = new DbmlTable(tableName);

        base.WalkTableDeclaration(syntax);

        _database.AddTable(_currentTable);
        _currentTable = null;
    }

#pragma warning disable CA1502 // Avoid excessive complexity
    /// <inheritdoc/>
    protected override void WalkColumnDeclarationStatement(ColumnDeclarationSyntax syntax)
    {
        string columnName = $"{syntax.IdentifierToken.Value ?? syntax.IdentifierToken.Text}";
        _currentTableColumn = new DbmlTableColumn(columnName, _currentTable);

        WalkColumnType(syntax.ColumnTypeClause);

        if (syntax.SettingList is not null)
        {
            foreach (ColumnSettingClause setting in syntax.SettingList.Settings)
            {
                switch (setting.Kind)
                {
                    case SyntaxKind.PrimaryKeyColumnSettingClause:
                    case SyntaxKind.PkColumnSettingClause:
                    {
                        _currentTableColumn.IsPrimaryKey = true;
                        break;
                    }
                    case SyntaxKind.NullColumnSettingClause:
                    {
                        _currentTableColumn.IsNullable = true;
                        break;
                    }
                    case SyntaxKind.NotNullColumnSettingClause:
                    {
                        _currentTableColumn.IsNullable = false;
                        break;
                    }
                    case SyntaxKind.UniqueColumnSettingClause:
                    {
                        _currentTableColumn.IsUnique = true;
                        break;
                    }
                    case SyntaxKind.IncrementColumnSettingClause:
                    {
                        _currentTableColumn.IsAutoIncrement = true;
                        break;
                    }
                    case SyntaxKind.DefaultColumnSettingClause:
                    {
                        DefaultColumnSettingClause defaultSetting = (DefaultColumnSettingClause)setting;
                        string defaultValue =
                            defaultSetting.ValueToken.Value?.ToString()
                                ?? defaultSetting.ValueToken.Text;
                        _currentTableColumn.DefaultValue = defaultValue;
                        break;
                    }
                    case SyntaxKind.NoteColumnSettingClause:
                    {
                        NoteColumnSettingClause noteSetting = (NoteColumnSettingClause)setting;
                        string noteValue =
                            noteSetting.ValueToken.Value?.ToString()
                                ?? noteSetting.ValueToken.Text;
                        _currentTableColumn.AddNote(noteValue);
                        break;
                    }
                    case SyntaxKind.UnknownColumnSettingClause:
                    {
                        UnknownColumnSettingClause unknownSetting = (UnknownColumnSettingClause)setting;
                        string settingName = unknownSetting.NameToken.Text;
                        string? settingValue =
                            unknownSetting.ValueToken?.Value?.ToString()
                                ?? unknownSetting.ValueToken?.Text
                                ?? null;

                        _currentTableColumn.AddUnknownSetting(settingName, settingValue);
                        break;
                    }
                    case SyntaxKind.RelationshipColumnSettingClause:
                    {
                        RelationshipColumnSettingClause relationshipSetting = (RelationshipColumnSettingClause)setting;
                        RelationshipConstraintClause constraintClause = relationshipSetting.ConstraintClause;

                        TableRelationshipType relationshipType =
                            constraintClause.RelationshipTypeToken.Kind switch
                            {
                                SyntaxKind.LessToken => TableRelationshipType.OneToMany,
                                SyntaxKind.GraterToken => TableRelationshipType.ManyToOne,
                                SyntaxKind.MinusToken => TableRelationshipType.OneToOne,
                                SyntaxKind.LessGraterToken => TableRelationshipType.ManyToMany,
                                _ => TableRelationshipType.OneToOne
                            };

                        string toRelation = constraintClause.ToIdentifier.ToString();
                        DbmlColumnIdentifier toColumn =
                            new DbmlColumnIdentifier(
                                schemaName: constraintClause.ToIdentifier.SchemaIdentifier?.Text ?? string.Empty,
                                tableName: constraintClause.ToIdentifier.TableIdentifier?.Text ?? string.Empty,
                                columnName: constraintClause.ToIdentifier.ColumnIdentifier?.Text ?? string.Empty);

                        _currentTableColumn.AddRelationship(relationshipType, toColumn);
                        break;
                    }
                    default:
                        throw new EvaluateException($"ERROR: Unknown syntax kind <{setting.Kind}>.");
                }
            }
        }

        _currentTable?.AddColumn(_currentTableColumn);
        _currentTableColumn = null;
    }
#pragma warning restore CA1502 // Avoid excessive complexity

#pragma warning disable CA1502 // Avoid excessive complexity
    private void WalkColumnType(ColumnTypeClause syntax)
    {
        if (_currentTableColumn is null)
            return;

        switch (syntax.Kind)
        {
            case SyntaxKind.ColumnTypeIdentifierClause:
            {
                ColumnTypeIdentifierClause columnTypeIdentifierClause =
                    (ColumnTypeIdentifierClause)syntax;

                string typeText = columnTypeIdentifierClause.ColumnTypeIdentifier.Text;
                _currentTableColumn.Type = typeText;

                switch (typeText)
                {
                    case "bit":
                        _currentTableColumn.MaxLength = 1;
                        _currentTableColumn.DefaultValue = "false";
                        return;
                    case "int":
                        _currentTableColumn.MaxLength = int.MaxValue;
                        _currentTableColumn.DefaultValue = "0";
                        return;
                    case "float":
                        _currentTableColumn.MaxLength = float.MaxValue;
                        _currentTableColumn.DefaultValue = "0";
                        return;
                }

                break;
            }

            case SyntaxKind.ColumnTypeParenthesizedIdentifierClause:
            {
                ColumnTypeParenthesizedIdentifierClause columnTypeIdentifierClause =
                    (ColumnTypeParenthesizedIdentifierClause)syntax;

                string typeText = columnTypeIdentifierClause.ColumnTypeIdentifier.Text;
                _currentTableColumn.Type =
                    $"{columnTypeIdentifierClause.ColumnTypeIdentifier.Text}({columnTypeIdentifierClause.VariableLengthIdentifier.Text})";

                switch (typeText)
                {
                    case "binary":
                    case "varbinary":
                    case "char":
                    case "varchar":
                    case "nchar":
                    case "nvarchar":
                    {
                        string variableLengthText =
                            columnTypeIdentifierClause.VariableLengthIdentifier.Text;

                        if (variableLengthText.Equals("MAX", StringComparison.OrdinalIgnoreCase))
                            _currentTableColumn.MaxLength = double.MaxValue;
                        else if (int.TryParse(variableLengthText, out int iMax))
                            _currentTableColumn.MaxLength = iMax;
                        else if (float.TryParse(variableLengthText, out float fMax))
                            _currentTableColumn.MaxLength = fMax;
                        else if (double.TryParse(variableLengthText, out double dMax))
                            _currentTableColumn.MaxLength = dMax;

                        return;
                    }
                    case "datetimeoffset":
                    {
                        _currentTableColumn.MaxLength = DateTimeOffset.MaxValue.Ticks;
                        _currentTableColumn.DefaultValue = "0";
                        return;
                    }
                }

                break;
            }

            default:
                throw new EvaluateException($"ERROR: Unknown syntax kind <{syntax.Kind}>.");
        }
    }
#pragma warning restore CA1502 // Avoid excessive complexity

    /// <inheritdoc/>
    protected override void WalkSingleFieldIndexDeclarationStatement(SingleFieldIndexDeclarationSyntax syntax)
    {
        string indexName = syntax.IdentifierToken.Text;
        string columnName = syntax.IdentifierToken.Text;
        _currentTableIndex = new DbmlTableIndex(indexName, columnName, _currentTable);

        base.WalkSingleFieldIndexDeclarationStatement(syntax);

        _currentTable?.AddIndex(_currentTableIndex);
        _currentTableIndex = null;
    }

    /// <inheritdoc/>
    protected override void WalkNoteDeclarationStatement(NoteDeclarationSyntax syntax)
    {
        string noteText = $"{syntax.Note.Value ?? syntax.Note.Text}";

        if (_currentTable is not null)
            _currentTable.AddNote(noteText);
        else if (_currentProject is not null)
            _currentProject.AddNote(noteText);
        else
            _database.AddNote(noteText);

        base.WalkNoteDeclarationStatement(syntax);
    }

    /// <inheritdoc/>
    protected override void WalkLiteralExpression(LiteralExpressionSyntax syntax)
    {
        base.WalkLiteralExpression(syntax);
    }

    /// <inheritdoc/>
    protected override void WalkNameExpression(NameExpressionSyntax syntax)
    {
        base.WalkNameExpression(syntax);
    }

    /// <inheritdoc/>
    protected override void WalkIndexSettingExpression(IndexSettingExpressionSyntax syntax)
    {
        if (_currentTableIndex is null)
            return;

        SyntaxToken[] identifiers = syntax.Identifiers.ToArray();
        if (identifiers.Length <= 0)
            return;

        // match by kind
        switch (identifiers[0].Kind)
        {
            case SyntaxKind.NameKeyword:
                SyntaxToken nameToken = syntax.Identifiers.Last();
                string name = $"{nameToken.Value ?? nameToken.Text}";
                _currentTableIndex.Name = string.IsNullOrEmpty(name) ? _currentTableIndex.Name : name;
                return;
            case SyntaxKind.NoteKeyword:
                SyntaxToken noteToken = syntax.Identifiers.Last();
                string note = $"{noteToken.Value ?? noteToken.Text}";
                _currentTableIndex.Note = string.IsNullOrEmpty(note) ? null : note;
                return;
            case SyntaxKind.TypeKeyword:
                SyntaxToken typeToken = syntax.Identifiers.Last();
                string type = $"{typeToken.Value ?? typeToken.Text}";
                _currentTableIndex.Type = string.IsNullOrEmpty(type) ? null : type;
                return;
        }

        // match by text
        string settingValue = string.Join("", identifiers.Select(i => i.Text));
        switch (settingValue)
        {
            case "pk":
            case "primarykey":
                _currentTableIndex.IsPrimaryKey = true;
                return;
            case "unique":
                _currentTableIndex.IsUnique = true;
                return;
            default:
                _currentTableIndex.AddSetting(settingValue);
                return;
        }
    }

    /// <inheritdoc/>
    protected override void WalkParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
    {
        base.WalkParenthesizedExpression(syntax);
    }
}
