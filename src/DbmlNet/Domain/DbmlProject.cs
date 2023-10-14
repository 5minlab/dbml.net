using System.Collections.Generic;

namespace DbmlNet.Domain;

/// <summary>
/// Represents a project setting clause in the syntax tree.
/// </summary>
public sealed class DbmlProject
{
    private readonly List<string> _notes = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DbmlProject"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the project.</param>
    public DbmlProject(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the name of the project.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the notes.
    /// </summary>
    public string Note => string.Join(separator: ", ", _notes);

    /// <summary>
    /// Gets the notes.
    /// </summary>
    public IEnumerable<string> Notes => _notes;

    /// <summary>
    /// Returns the project name.
    /// </summary>
    /// <returns>The project name.</returns>
    public override string ToString() => Name;

    internal void AddNote(string note)
    {
        if (!string.IsNullOrEmpty(note))
            _notes.Add(note);
    }
}
