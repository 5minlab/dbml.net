using DbmlNet.CodeAnalysis.Text;

using Xunit;

namespace DbmlNet.Tests.Unit.CodeAnalysis.Syntax;

public sealed class SourceTextTests
{
    [Fact]
    public void SourceText_From_Creates_SourceText_With_Empty_Text()
    {
        const string inputText = "";

        SourceText text = SourceText.From(inputText);

        Assert.Equal(inputText, text.ToString());
        Assert.True(0 == text.Length, $"Expected 0 == text.Length, and got {text.Length} ");
        TextLine line = Assert.Single(text.Lines);
        Assert.Equal("", line.ToString());
        Assert.True(0 == line.Start, $"Expected 0 == line.Start, and got {line.Start}");
        Assert.True(0 == line.Length, $"Expected 0 == line.Length, and got {line.Length}");
        Assert.True(0 == line.End, $"Expected 0 == line.End, and got {line.End}");
        Assert.True(0 == line.LengthIncludingLineBreak, $"Expected 0 == line.LengthIncludingLineBreak, and got {line.LengthIncludingLineBreak}");
        Assert.True(0 == line.Span.Start, $"Expected 0 == line.Span.Start, and got {line.Span.Start}");
        Assert.True(0 == line.Span.Length, $"Expected 0 == line.Span.Length, and got {line.Span.Length}");
        Assert.True(0 == line.Span.End, $"Expected 0 == line.Span.End, and got {line.Span.End}");
        Assert.True(0 == line.SpanIncludingLineBreak.Start, $"Expected 0 == line.SpanIncludingLineBreak.Start, and got {line.SpanIncludingLineBreak.Start}");
        Assert.True(0 == line.SpanIncludingLineBreak.Length, $"Expected 0 == line.SpanIncludingLineBreak.Length, and got {line.SpanIncludingLineBreak.Length}");
        Assert.True(0 == line.SpanIncludingLineBreak.End, $"Expected 0 == line.SpanIncludingLineBreak.End, and got {line.SpanIncludingLineBreak.End}");
    }
}
