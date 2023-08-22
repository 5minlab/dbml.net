using System;
using System.Collections.Generic;

namespace DbmlNet.Domain;

/// <summary>
/// </summary>
public sealed class DbmlTableColumn
{
    private readonly List<string> _notes = new();
    private readonly List<(string name, object? value)> _unknownSettings = new();

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="table"></param>
    public DbmlTableColumn(string name, DbmlTable? table)
    {
        Name = name;
        Table = table;
    }

    /// <summary>
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// </summary>
    public string? Type { get; internal set; }

    /// <summary>
    /// </summary>
    public double? MaxLength { get; internal set; }

    /// <summary>
    /// </summary>
    public bool HasMaxLength => MaxLength == double.MaxValue;

    /// <summary>
    /// </summary>
    public bool IsPrimaryKey { get; internal set; }

    /// <summary>
    /// </summary>
    public bool IsUnique { get; internal set; }

    /// <summary>
    /// </summary>
    public bool IsAutoIncrement { get; internal set; }

    /// <summary>
    /// </summary>
    public bool IsNullable { get; internal set; }

    /// <summary>
    /// </summary>
    public bool IsRequired => !IsNullable;

    /// <summary>
    /// </summary>
    public bool HasDefaultValue => DefaultValue is not null;

    /// <summary>
    /// </summary>
    public string? DefaultValue { get; internal set; }

    /// <summary>
    /// </summary>
    public DbmlTable? Table { get; }

    /// <summary>
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
    /// </summary>
    public IEnumerable<string> Notes => _notes;

    /// <summary>
    /// </summary>
    public IEnumerable<(string name, object? value)> UnknownSettings => _unknownSettings;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;

    internal void AddNote(string note)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(note);
        _notes.Add(note);
    }

    internal void AddUnknownSetting(string name, object? value)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);
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
