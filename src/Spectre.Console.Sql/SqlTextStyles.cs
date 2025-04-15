namespace Spectre.Console.Sql;

internal sealed class SqlTextStyles
{
    public Style StringValueStyle { get; set; } = default!;
    public Style NumberStyle { get; set; } = default!;
    public Style KeywordStyle { get; set; } = default!;
    public Style IdentifierStyle { get; set; } = default!;
    public Style OperatorStyle { get; set; } = default!;
    public Style CommaStyle { get; set; } = default!;
    public Style DotStyle { get; set; } = default!;
    public Style OpenParenthesisStyle { get; set; } = default!;
    public Style CloseParenthesisStyle { get; set; } = default!;
    public Style SequenceTerminatorStyle { get; set; } = default!;
    public Style NotDefinedStyle { get; set; } = default!;
    public Style CommentStyle { get; set; } = default!;
}
