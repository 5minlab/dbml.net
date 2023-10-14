using System;
using System.Collections.Generic;

namespace DbmlNet.Domain;

/// <summary>
/// Represents a table.
/// </summary>
public sealed class DbmlTable
{
    private readonly List<string> _notes = new();
    private readonly List<DbmlTableColumn> _columns = new();
    private readonly List<DbmlTableIndex> _indexes = new();
    private readonly List<DbmlTableRelationship> _relationships = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DbmlTable"/> class with the specified name and optional note.
    /// </summary>
    /// <param name="name">The name of the table.</param>
    /// <param name="note">An optional note for the table.</param>
    public DbmlTable(string name, string? note = null)
    {
        Name = name;

        if (note is not null)
            _notes.Add(note);
    }

    /// <summary>
    /// Gets the name of the table.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the columns.
    /// </summary>
    public IEnumerable<DbmlTableColumn> Columns => _columns;

    /// <summary>
    /// Gets the indexes.
    /// </summary>
    public IEnumerable<DbmlTableIndex> Indexes => _indexes;

    /// <summary>
    /// Gets the relationships.
    /// </summary>
    public IEnumerable<DbmlTableRelationship> Relationships => _relationships;

    /// <summary>
    /// Gets the notes.
    /// </summary>
    public string Note => string.Join(separator: ", ", _notes);

    /// <summary>
    /// Gets the notes.
    /// </summary>
    public IEnumerable<string> Notes => _notes;

    /// <summary>
    /// Returns the table name.
    /// </summary>
    /// <returns>The table name.</returns>
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
        ArgumentException.ThrowIfNullOrEmpty(note);
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
