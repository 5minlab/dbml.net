using System;
using System.Collections.Generic;

namespace DbmlNet.Domain;

/// <summary>
/// Represents a column setting clause in the syntax tree.
/// </summary>
public sealed class DbmlTableColumn
{
    private readonly List<string> _notes = new();
    private readonly List<(string name, object? value)> _unknownSettings = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DbmlTableColumn"/> class with the specified name and table.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <param name="table">The <see cref="DbmlTable"/> instance that the column belongs to.</param>
    public DbmlTableColumn(string name, DbmlTable? table)
    {
        Name = name;
        Table = table;
    }

    /// <summary>
    /// Gets the name of the column.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the table.
    /// </summary>
    public string? Type { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public double? MaxLength { get; internal set; }

    /// <summary>
    /// A flag indicating if the column has a max length.
    /// </summary>
    public bool HasMaxLength => MaxLength == double.MaxValue;

    /// <summary>
    /// A flag indicating if the column is a primary key.
    /// </summary>
    public bool IsPrimaryKey { get; internal set; }

    /// <summary>
    /// A flag indicating if the column is unique.
    /// </summary>
    public bool IsUnique { get; internal set; }

    /// <summary>
    /// A flag indicating if the column is auto incremented.
    /// </summary>
    public bool IsAutoIncrement { get; internal set; }

    /// <summary>
    /// A flag indicating if the column is nullable.
    /// </summary>
    public bool IsNullable { get; internal set; }

    /// <summary>
    /// A flag indicating if the column is required.
    /// </summary>
    public bool IsRequired => !IsNullable;

    /// <summary>
    /// A flag indicating if the column has a default value.
    /// </summary>
    public bool HasDefaultValue => DefaultValue is not null;

    /// <summary>
    /// Gets the default value.
    /// </summary>
    public string? DefaultValue { get; internal set; }

    /// <summary>
    /// Gets the table.
    /// </summary>
    public DbmlTable? Table { get; }

    /// <summary>
    /// Gets the notes.
    /// </summary>
    public string? Note
    {
        get
        {
            string note = string.Join("; ", _notes.ToArray()).Trim();
            return string.IsNullOrEmpty(note)
                ? null
                : note;
        }
    }

    /// <summary>
    /// Gets the notes.
    /// </summary>
    public IEnumerable<string> Notes => _notes;

    /// <summary>
    /// Gets the unknown settings.
    /// </summary>
    public IEnumerable<(string name, object? value)> UnknownSettings => _unknownSettings;

    /// <summary>
    /// Returns the name of this column.
    /// </summary>
    /// <returns>The name of this column.</returns>
    public override string ToString() => Name;

    internal void AddNote(string note)
    {
        ArgumentException.ThrowIfNullOrEmpty(note);
        _notes.Add(note);
    }

    internal void AddUnknownSetting(string name, object? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        _unknownSettings.Add((name, value));
    }

    internal void AddRelationship(
        TableRelationshipType relationshipType,
        DbmlColumnIdentifier toColumn)
    {
        DbmlColumnIdentifier fromColumn =
            new DbmlColumnIdentifier(
                schemaName: string.Empty,
                tableName: Table?.Name ?? string.Empty,
                columnName: Name);

        Table?.AddRelationship(fromColumn, relationshipType, toColumn);
    }
}
