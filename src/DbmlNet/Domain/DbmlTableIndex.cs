using System;
using System.Collections.Generic;

namespace DbmlNet.Domain;

/// <summary>
/// Represents a table index setting clause in the syntax tree.
/// </summary>
public sealed class DbmlTableIndex
{
    private readonly List<string> _settings = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DbmlTableIndex"/>.
    /// </summary>
    /// <param name="name">The name of the table index.</param>
    /// <param name="columnName">The name of the column.</param>
    /// <param name="table">The <see cref="DbmlTable"/> associated with the index (optional).</param>
    public DbmlTableIndex(string name, string columnName, DbmlTable? table = null)
    {
        Name = name;
        ColumnName = columnName;
        Table = table;
    }

    /// <summary>
    /// Gets the name of the table index.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the name of the column.
    /// </summary>
    public string ColumnName { get; }

    /// <summary>
    /// Gets the <see cref="DbmlTable"/> associated with the index (optional).
    /// </summary>
    public DbmlTable? Table { get; }

    /// <summary>
    /// A flag indicating if the index is a primary key.
    /// </summary>
    public bool IsPrimaryKey { get; internal set; }

    /// <summary>
    /// A flag indicating if the index is unique.
    /// </summary>
    public bool IsUnique { get; internal set; }

    /// <summary>
    /// A flag indicating if the index is auto incremented.
    /// </summary>
    public string? Type { get; internal set; }

    /// <summary>
    /// Gets the note.
    /// </summary>
    public string? Note { get; internal set; }

    /// <summary>
    /// Gets the settings.
    /// </summary>
    public IEnumerable<string> Settings => _settings;

    /// <summary>
    /// Returns the name of this index.
    /// </summary>
    /// <returns>The name of this index.</returns>
    public override string ToString() => Name;

    internal void AddSetting(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        _settings.Add(value);
    }
}
