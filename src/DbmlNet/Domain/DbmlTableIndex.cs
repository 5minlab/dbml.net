using System;
using System.Collections.Generic;

namespace DbmlNet.Domain;

/// <summary>
/// </summary>
public sealed class DbmlTableIndex
{
    private readonly List<string> _settings = new();

    /// <summary>
    /// </summary>
    /// <param name="name">The name of the index.</param>
    /// <param name="columnName">The column name of the index.</param>
    /// <param name="table">The table which contains the index.</param>
    public DbmlTableIndex(string name, string columnName, DbmlTable? table = null)
    {
        Name = name;
        ColumnName = columnName;
        Table = table;
    }

    /// <summary>
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// </summary>
    public string ColumnName { get; }

    /// <summary>
    /// </summary>
    public DbmlTable? Table { get; }

    /// <summary>
    /// </summary>
    public bool IsPrimaryKey { get; internal set; }

    /// <summary>
    /// </summary>
    public bool IsUnique { get; internal set; }

    /// <summary>
    /// </summary>
    public string? Type { get; internal set; }

    /// <summary>
    /// </summary>
    public string? Note { get; internal set; }

    /// <summary>
    /// </summary>
    public IEnumerable<string> Settings => _settings;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;

    internal void AddSetting(string value)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(value);
        _settings.Add(value);
    }
}
