using System;
using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;

namespace DbmlNet.Domain;

/// <summary>
/// Represents a dbml database.
/// </summary>
public sealed class DbmlDatabase
{
    private readonly List<string> _notes = new();
    private readonly List<string> _databaseProviders = new();
    private readonly List<DbmlTable> _tables = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DbmlDatabase"/> class.
    /// </summary>
    public DbmlDatabase()
    {
    }

    /// <summary>
    /// Gets the list of database providers.
    /// </summary>
    public IEnumerable<string> Providers => _databaseProviders;

    /// <summary>
    /// Gets or sets the list of tables.
    /// </summary>
    public DbmlProject? Project { get; set; }

    /// <summary>
    /// Gets the list of tables.
    /// </summary>
    public IEnumerable<DbmlTable> Tables => _tables;

    /// <summary>
    /// Gets the list of notes.
    /// </summary>
    public string Note => string.Join(separator: ", ", _notes);

    /// <summary>
    /// Gets the list of notes.
    /// </summary>
    public IEnumerable<string> Notes => _notes;

    /// <summary>
    /// Creates a <see cref="DbmlDatabase"/> from a given <see cref="SyntaxTree"/>.
    /// </summary>
    /// <param name="syntaxTree">The <see cref="SyntaxTree"/> representing the parsed dbml code.</param>
    /// <returns>A <see cref="DbmlDatabase"/> object representing the parsed code.</returns>
    public static DbmlDatabase Create(SyntaxTree syntaxTree)
    {
        return DbmlDatabaseMaker.Make(syntaxTree);
    }

    /// <summary>
    /// Adds a <see cref="DbmlTable"/> to the list of tables.
    /// </summary>
    /// <param name="table">The table to add.</param>
    internal void AddTable(DbmlTable table)
    {
        ArgumentNullException.ThrowIfNull(table);
        _tables.Add(table);
    }

    /// <summary>
    /// Adds a provider to the list of database providers.
    /// </summary>
    /// <param name="providerName">The name of the provider to add.</param>
    internal void AddProvider(string providerName)
    {
        ArgumentException.ThrowIfNullOrEmpty(providerName);
        _databaseProviders.Add(providerName);
    }

    /// <summary>
    /// Adds a note to the list of database notes.
    /// </summary>
    /// <param name="note">The note to add.</param>
    internal void AddNote(string note)
    {
        ArgumentException.ThrowIfNullOrEmpty(note);
        _notes.Add(note);
    }
}
