using System;
using System.Diagnostics;
using System.Globalization;

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

    public static DbmlDatabase Make(SyntaxTree syntaxTree)
    {
        DbmlDatabaseMaker maker = new(syntaxTree);
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

        string projectName = $"{syntax.IdentifierToken.Value}";
        _currentProject = new DbmlProject(projectName);

        foreach (ProjectSettingClause setting in syntax.Settings.Settings)
        {
            if (setting.Kind == SyntaxKind.DatabaseProviderProjectSettingClause)
            {
                DatabaseProviderProjectSettingClause databaseProviderSetting =
                    (DatabaseProviderProjectSettingClause)setting;
                string providerName = $"{databaseProviderSetting.ValueToken.Value}";
                _database.AddProvider(providerName);
            }
            else if (setting.Kind == SyntaxKind.NoteProjectSettingClause)
            {
                NoteProjectSettingClause noteSetting = (NoteProjectSettingClause)setting;
                string note = $"{noteSetting.ValueToken.Value}";
                _currentProject.AddNote(note);
            }
            else
            {
                Debug.Assert(condition: false, $"Unknown project setting kind '{setting.Kind}'.");
            }
        }

        _database.Project = _currentProject;
        _currentProject = null;
    }

    /// <inheritdoc/>
    protected override void WalkTableDeclaration(TableDeclarationSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        string tableName = syntax.DbSchema.TableIdentifier.Text;
        string? schemaName = syntax.DbSchema.SchemaIdentifier?.Text;
        string? databaseName = syntax.DbSchema.DatabaseIdentifier?.Text;
        _currentTable = new DbmlTable(tableName, schemaName, databaseName);

        base.WalkTableDeclaration(syntax);

        _database.AddTable(_currentTable);
        _currentTable = null;
    }

#pragma warning disable CA1502 // Avoid excessive complexity
#pragma warning disable MA0051 // Method is too long (maximum allowed: 60)

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
                        WalkExpression(defaultSetting.ExpressionValue);
                        break;
                    }

                    case SyntaxKind.NoteColumnSettingClause:
                    {
                        NoteColumnSettingClause noteSetting = (NoteColumnSettingClause)setting;
                        string noteValue = $"{noteSetting.ValueToken.Value}";
                        if (!string.IsNullOrEmpty(noteValue))
                            _currentTableColumn.AddNote(noteValue);

                        break;
                    }

                    case SyntaxKind.UnknownColumnSettingClause:
                    {
                        UnknownColumnSettingClause unknownSetting = (UnknownColumnSettingClause)setting;
                        string settingName = unknownSetting.NameToken.Text;
                        string? settingValue =
                            unknownSetting.ValueToken?.Value?.ToString()
                                ?? unknownSetting.ValueToken?.Text;

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
                                SyntaxKind.LessGraterToken => TableRelationshipType.ManyToMany,
                                _ => TableRelationshipType.OneToOne
                            };

                        string? toSchemaName = constraintClause.ToIdentifier.SchemaIdentifier?.Text;
                        string? toTableName = constraintClause.ToIdentifier.TableIdentifier?.Text;
                        string toColumnName = constraintClause.ToIdentifier.ColumnIdentifier.Text;

                        DbmlColumnIdentifier toColumn = new(toSchemaName, toTableName, toColumnName);

                        _currentTableColumn.AddRelationship(relationshipType, toColumn);
                        break;
                    }

                    default:
                    {
                        Debug.Assert(condition: false, $"Unknown column setting kind '{setting.Kind}'.");
                        break;
                    }
                }
            }
        }

        Debug.Assert(_currentTable is not null, "Current table should not be null.");
        _currentTable.AddColumn(_currentTableColumn);
        _currentTableColumn = null;
    }
#pragma warning restore CA1502 // Avoid excessive complexity
#pragma warning restore MA0051 // Method is too long (maximum allowed: 60)

#pragma warning disable MA0051 // Method is too long (maximum allowed: 60)

    /// <inheritdoc/>
    protected override void WalkSingleFieldIndexDeclarationStatement(SingleFieldIndexDeclarationSyntax syntax)
    {
        string indexName = syntax.IdentifierToken.Text;
        string columnName = syntax.IdentifierToken.Text;
        _currentTableIndex = new DbmlTableIndex(indexName, columnName, _currentTable);

        if (syntax.Settings is not null)
        {
            foreach (IndexSettingClause setting in syntax.Settings.Settings)
            {
                switch (setting.Kind)
                {
                    case SyntaxKind.PrimaryKeyIndexSettingClause:
                    case SyntaxKind.PkIndexSettingClause:
                    {
                        _currentTableIndex.IsPrimaryKey = true;
                        break;
                    }

                    case SyntaxKind.UniqueIndexSettingClause:
                    {
                        _currentTableIndex.IsUnique = true;
                        break;
                    }

                    case SyntaxKind.TypeIndexSettingClause:
                    {
                        TypeIndexSettingClause typeSetting = (TypeIndexSettingClause)setting;
                        _currentTableIndex.Type = $"{typeSetting.ValueToken.Value ?? typeSetting.ValueToken.Text}";
                        break;
                    }

                    case SyntaxKind.NameIndexSettingClause:
                    {
                        NameIndexSettingClause nameSetting = (NameIndexSettingClause)setting;
                        _currentTableIndex.Name = $"{nameSetting.ValueToken.Value}";
                        break;
                    }

                    case SyntaxKind.NoteIndexSettingClause:
                    {
                        NoteIndexSettingClause noteSetting = (NoteIndexSettingClause)setting;
                        _currentTableIndex.Note = $"{noteSetting.ValueToken.Value}";
                        break;
                    }

                    case SyntaxKind.UnknownIndexSettingClause:
                    {
                        UnknownIndexSettingClause unknownSetting = (UnknownIndexSettingClause)setting;
                        string settingName = unknownSetting.NameToken.Text;
                        string? settingValue =
                            unknownSetting.ValueToken?.Value?.ToString()
                                ?? unknownSetting.ValueToken?.Text;

                        _currentTableIndex.AddUnknownSetting(settingName, settingValue);
                        break;
                    }

                    default:
                    {
                        Debug.Assert(condition: false, $"Unknown index setting kind '{setting.Kind}'.");
                        break;
                    }
                }
            }
        }

        Debug.Assert(_currentTable is not null, "Current table should not be null.");
        _currentTable.AddIndex(_currentTableIndex);
        _currentTableIndex = null;
    }

#pragma warning restore MA0051 // Method is too long (maximum allowed: 60)

    /// <inheritdoc/>
    protected override void WalkNoteDeclarationStatement(NoteDeclarationSyntax syntax)
    {
        string noteText = $"{syntax.Note.Value}";

        if (_currentTable is not null)
            _currentTable.AddNote(noteText);
        else
            _database.AddNote(noteText);

        base.WalkNoteDeclarationStatement(syntax);
    }

    /// <inheritdoc/>
    protected override void WalkLiteralExpression(LiteralExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        if (_currentTableColumn is not null)
        {
            string defaultValue = $"{syntax.LiteralToken.Value}";
            _currentTableColumn.DefaultValue = defaultValue switch
            {
                "False" => "false",
                "True" => "true",
                _ => defaultValue
            };
        }
    }

    /// <inheritdoc/>
    protected override void WalkNameExpression(NameExpressionSyntax syntax)
    {
        ArgumentNullException.ThrowIfNull(syntax);

        if (_currentTableColumn is not null)
        {
            _currentTableColumn.DefaultValue = syntax.IdentifierToken.Text;
        }
    }

#pragma warning disable CA1502 // Avoid excessive complexity
#pragma warning disable MA0051 // Method is too long (maximum allowed: 60)

    private void WalkColumnType(ColumnTypeClause syntax)
    {
        Debug.Assert(_currentTableColumn is not null, "Current column should not be null");

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
                    default:
                    {
                        return;
                    }
                }
            }

            case SyntaxKind.ColumnTypeParenthesizedIdentifierClause:
            {
                ColumnTypeParenthesizedIdentifierClause columnTypeIdentifierClause =
                    (ColumnTypeParenthesizedIdentifierClause)syntax;

                string typeText = columnTypeIdentifierClause.ColumnTypeIdentifier.Text;
                string variableLengthText = columnTypeIdentifierClause.VariableLengthIdentifier.Text;
                _currentTableColumn.Type = columnTypeIdentifierClause.Text;

                switch (typeText)
                {
                    case "binary":
                    case "varbinary":
                    case "char":
                    case "varchar":
                    case "nchar":
                    case "nvarchar":
                    {
                        if (variableLengthText.Equals("MAX", StringComparison.OrdinalIgnoreCase))
                            _currentTableColumn.MaxLength = double.MaxValue;
                        else if (int.TryParse(variableLengthText, NumberFormatInfo.InvariantInfo, out int iMax))
                            _currentTableColumn.MaxLength = iMax;
                        else if (double.TryParse(variableLengthText, NumberFormatInfo.InvariantInfo, out double dMax))
                            _currentTableColumn.MaxLength = dMax;

                        return;
                    }

                    case "datetimeoffset":
                    {
                        _currentTableColumn.MaxLength = DateTimeOffset.MaxValue.Ticks;
                        _currentTableColumn.DefaultValue = "0";
                        return;
                    }

                    default:
                    {
                        return;
                    }
                }
            }

            default:
                Debug.Assert(condition: false, $"ERROR: Unknown syntax kind <{syntax.Kind}>.");
                break;
        }
    }

#pragma warning restore CA1502 // Avoid excessive complexity
#pragma warning restore MA0051 // Method is too long (maximum allowed: 60)

}
