using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis;

/// <summary>
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
    /// </summary>
    public bool IsError { get; }

    /// <summary>
    /// </summary>
    public TextLocation Location { get; }

    /// <summary>
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// </summary>
    public bool IsWarning { get; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Message;

    /// <summary>
    /// </summary>
    /// <param name="location"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Diagnostic Error(TextLocation location, string message)
    {
        return new Diagnostic(isError: true, location, message);
    }

    /// <summary>
    /// </summary>
    /// <param name="location"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Diagnostic Warning(TextLocation location, string message)
    {
        return new Diagnostic(isError: false, location, message);
    }
}
