#nullable enable

using System;

namespace DbmlNet;

public static class MyArgumentNullExceptionExt
{
    public static void ThrowIfNull(object? argument)
    {
        if (argument == null)
            throw new ArgumentNullException(nameof(argument), "argument is null");
    }
}

public static class MyArgumentExceptionExt
{
    public static void ThrowIfNullOrEmpty(string? note)
    {
        if (string.IsNullOrEmpty(note))
            throw new ArgumentNullException(nameof(note), "note is null or empty");
    }
}

