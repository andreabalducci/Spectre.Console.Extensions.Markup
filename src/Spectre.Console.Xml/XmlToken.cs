namespace Spectre.Console.Xml;
internal readonly struct XmlToken(TextSpan span, XmlSyntaxKind kind)
{
    public TextSpan Span { get; } = span;
    public XmlSyntaxKind Kind { get; } = kind;

    public override string ToString() => $"{Span.Start} {Span.End} - {Kind}";
}
