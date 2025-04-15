namespace Spectre.Console.Sql;
internal enum SqlSyntaxKind
{
    StringValue,
    Number,
    Keyword,
    Identifier,
    Operator,
    Comma,
    Dot,
    OpenParenthesis,
    CloseParenthesis,
    SequenceTerminator,
    NotDefined,
    Comment
}