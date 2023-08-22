using System;
using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;

namespace DbmlNet.Domain;

/// <summary>
/// </summary>
public sealed class DbmlDatabase
{
    private readonly List<string> _notes = new();
    private readonly List<string> _databaseProviders = new();
    private readonly List<DbmlTable> _tables = new();

    /// <summary>
    /// </summary>
    public DbmlDatabase()
    {
    }

    /// <summary>
    /// </summary>
    public IEnumerable<string> Providers => _databaseProviders;

    /// <summary>
    /// </summary>
    public DbmlProject? Project { get; set; }

    /// <summary>
    /// </summary>
    public IEnumerable<DbmlTable> Tables => _tables;

    /// <summary>
    /// </summary>
    public string Note => string.Join(separator: ", ", _notes);

    /// <summary>
    /// </summary>
    public IEnumerable<string> Notes => _notes;

    /// <summary>
    /// </summary>
    /// <param name="syntaxTree"></param>
    /// <returns></returns>
    public static DbmlDatabase Create(SyntaxTree syntaxTree)
    {
        return DbmlDatabaseMaker.Make(syntaxTree);
    }

    internal void AddTable(DbmlTable currentTable)
    {
        ArgumentNullException.ThrowIfNull(currentTable);
        _tables.Add(currentTable);
    }

    internal void AddProvider(string providerName)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(providerName);
        _databaseProviders.Add(providerName);
    }

    internal void AddNote(string note)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(note);
        _notes.Add(note);
    }
}
