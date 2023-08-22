using System.IO;

namespace DbmlNet.IO;

internal static class TextWriterExtensions
{
    public static void WriteInfoMessage(this TextWriter writer, string message)
    {
        writer.WriteInformation("Info: ");
        writer.WriteInformation(message);
        writer.WriteLine();
    }

    public static void WriteOkMessage(this TextWriter writer, string message)
    {
        writer.WriteSuccess("Ok: ");
        writer.WriteSuccess(message);
        writer.WriteLine();
    }

    public static void WriteWarningMessage(this TextWriter writer, string message)
    {
        writer.WriteWarning("Warn: ");
        writer.WriteWarning(message);
        writer.WriteLine();
    }

    public static void WriteErrorMessage(this TextWriter writer, string message)
    {
        writer.WriteError("Error: ");
        writer.WriteError(message);
        writer.WriteLine();
    }
}
