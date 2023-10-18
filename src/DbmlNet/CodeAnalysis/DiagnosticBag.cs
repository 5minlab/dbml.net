
using System;
using System.Collections;
using System.Collections.Generic;

using DbmlNet.CodeAnalysis.Syntax;
using DbmlNet.CodeAnalysis.Text;

namespace DbmlNet.CodeAnalysis;

internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddRange(IEnumerable<Diagnostic> diagnostics)
    {
        _diagnostics.AddRange(diagnostics);
    }

    private void ReportError(TextLocation location, string message)
    {
        Diagnostic diagnostic = Diagnostic.Error(location, message);
        _diagnostics.Add(diagnostic);
    }

    private void ReportWarning(TextLocation location, string message)
    {
        Diagnostic diagnostic = Diagnostic.Warning(location, message);
        _diagnostics.Add(diagnostic);
    }

    public void ReportInvalidNumber(TextLocation location, string text, Type type)
    {
        string message = $"The number {text} isn't valid {type}.";
        ReportError(location, message);
    }

    public void ReportBadCharacter(TextLocation location, char character)
    {
        string message = $"Bad character input: '{character}'.";
        ReportError(location, message);
    }

    public void ReportNumberToLarge(TextLocation location, string numberText)
    {
        string message = $"The number '{numberText}' is too large.";
        ReportError(location, message);
    }

    public void ReportUnexpectedToken(TextLocation location, SyntaxKind currentKind, SyntaxKind expectedKind)
    {
        string message = $"Unexpected token <{currentKind}>, expected <{expectedKind}>.";
        ReportError(location, message);
    }

    public void ReportUnknownColumnSetting(TextLocation location, string settingName)
    {
        string message = $"Unknown column setting '{settingName}'.";
        ReportWarning(location, message);
    }

    public void ReportUnknownProjectSetting(TextLocation location, string settingName)
    {
        string message = $"Unknown project setting '{settingName}'.";
        ReportWarning(location, message);
    }

    public void ReportUnknownIndexSetting(TextLocation location, string settingName)
    {
        string message = $"Unknown index setting '{settingName}'.";
        ReportWarning(location, message);
    }

    public void ReportUnknownIndexSettingType(TextLocation location, string unknownType)
    {
        string message = $"Unknown index setting type '{unknownType}'. Allowed type of index ({string.Join(", ", Parser.IndexSettingTypes)} depending on DB).";
        ReportWarning(location, message);
    }
}
