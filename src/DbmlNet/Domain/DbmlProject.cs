using System.Collections.Generic;

namespace DbmlNet.Domain;

/// <summary>
/// </summary>
public sealed class DbmlProject
{
    private readonly List<string> _notes = new();

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    public DbmlProject(string name)
    {
        Name = name;
    }

    /// <summary>
    /// </summary>
    public string Name { get; }

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

    internal void AddNote(string note)
    {
        if (!string.IsNullOrEmpty(note))
            _notes.Add(note);
    }
}
