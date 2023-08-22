using System;
using System.Collections.Generic;

namespace DbmlNet.Domain;

/// <summary>
/// </summary>
public sealed class DbmlTable
{
    private readonly List<string> _notes = new();
    private readonly List<DbmlTableColumn> _columns = new();
    private readonly List<DbmlTableIndex> _indexes = new();
    private readonly List<DbmlTableRelationship> _relationships = new();

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="note"></param>
    public DbmlTable(string name, string? note = null)
    {
        Name = name;

        if (note is not null)
            _notes.Add(note);
    }

    /// <summary>
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// </summary>
    public IEnumerable<DbmlTableColumn> Columns => _columns;

    /// <summary>
    /// </summary>
    public IEnumerable<DbmlTableIndex> Indexes => _indexes;

    /// <summary>
    /// </summary>
    public IEnumerable<DbmlTableRelationship> Relationships => _relationships;

    /// <summary>
    /// </summary>
    public string Note => string.Join(separator: ", ", _notes);

    /// <summary>
    /// </summary>
    public IEnumerable<string> Notes => _notes;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;

    internal void AddColumn(DbmlTableColumn column)
    {
        ArgumentNullException.ThrowIfNull(column);
        _columns.Add(column);
    }

    internal void AddIndex(DbmlTableIndex index)
    {
        ArgumentNullException.ThrowIfNull(index);
        _indexes.Add(index);
    }

    internal void AddNote(string note)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(note);
        _notes.Add(note);
    }

    internal void AddRelationship(
        DbmlColumnIdentifier fromColumn,
        TableRelationshipType relationshipType,
        DbmlColumnIdentifier toColumn)
    {
        ArgumentNullException.ThrowIfNull(fromColumn);
        ArgumentNullException.ThrowIfNull(relationshipType);
        ArgumentNullException.ThrowIfNull(toColumn);

        DbmlTableRelationship relationship =
            new DbmlTableRelationship(fromColumn, relationshipType, toColumn);

        _relationships.Add(relationship);
    }
}
