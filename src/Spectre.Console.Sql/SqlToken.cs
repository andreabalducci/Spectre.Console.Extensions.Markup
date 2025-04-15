
namespace Spectre.Console.Sql;
internal readonly struct SqlToken(TextSpan span, SqlSyntaxKind kind)
{
    public TextSpan Span { get; } = span;
    public SqlSyntaxKind Kind { get; } = kind;

    public override string ToString() => $"{Span.Start} {Span.End} - {Kind}";
}
