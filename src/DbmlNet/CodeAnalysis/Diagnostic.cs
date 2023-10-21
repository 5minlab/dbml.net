using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis;

/// <summary>
/// Represents a diagnostic message.
/// </summary>
public sealed class Diagnostic
{
    private Diagnostic(bool isError, TextLocation location, string message)
    {
        IsError = isError;
        Location = location;
        Message = message;
        IsWarning = !IsError;
    }

    /// <summary>
    /// Gets a value indicating whether the diagnostic is an error.
    /// </summary>
    public bool IsError { get; }

    /// <summary>
    /// Gets location associated with the diagnostic.
    /// </summary>
    public TextLocation Location { get; }

    /// <summary>
    /// Gets the diagnostic message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets a value indicating whether the diagnostic is an warning.
    /// </summary>
    public bool IsWarning { get; }

    /// <summary>
    /// Creates a new error diagnostic with the specified location and message.
    /// </summary>
    /// <param name="location">The location of the error.</param>
    /// <param name="message">The error message.</param>
    /// <returns>A new error diagnostic.</returns>
    public static Diagnostic Error(TextLocation location, string message)
    {
        return new Diagnostic(isError: true, location, message);
    }

    /// <summary>
    /// Creates a warning diagnostic with the specified location and message.
    /// </summary>
    /// <param name="location">The location of the warning.</param>
    /// <param name="message">The warning message.</param>
    /// <returns>A warning diagnostic.</returns>
    public static Diagnostic Warning(TextLocation location, string message)
    {
        return new Diagnostic(isError: false, location, message);
    }

    /// <summary>
    /// Returns the diagnostic message.
    /// </summary>
    /// <returns>The diagnostic message.</returns>
    public override string ToString() => Message;
}
