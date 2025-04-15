namespace Spectre.Console.Javascript;
internal readonly struct JavascriptToken(TextSpan span, JavascriptSyntaxKind kind)
{
    public TextSpan Span { get; } = span;
    public JavascriptSyntaxKind Kind { get; } = kind;

    public override string ToString() => $"{Span.Start} {Span.End} - {Kind}";
}
