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
    /// <param name="table">The name of the table.</param>
    /// <param name="schema">The name of the schema.</param>
    /// <param name="database">The name of the database.</param>
    public DbmlTable(
        string table,
        string? schema = null,
        string? database = null)
    {
        Name = table;
        Schema = schema;
        Database = database;
    }

    /// <summary>
    /// Gets the name of the table.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the name of the schema.
    /// </summary>
    public string? Schema { get; }

    /// <summary>
    /// Gets the name of the database.
    /// </summary>
    public string? Database { get; }

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
    /// Returns the full text for this table identifier using format {database}.{schema}.{table}.
    /// </summary>
    /// <returns>The full text for this table identifier.</returns>
    public override string ToString()
    {
        if (!string.IsNullOrEmpty(Database) && !string.IsNullOrEmpty(Schema))
            return $"{Database}.{Schema}.{Name}";
        else if (!string.IsNullOrEmpty(Schema))
            return $"{Schema}.{Name}";
        else
            return Name;
    }

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
