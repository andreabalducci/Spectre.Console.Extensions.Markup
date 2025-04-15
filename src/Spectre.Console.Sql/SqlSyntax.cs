using System.Diagnostics;

namespace Spectre.Console.Sql;

internal sealed class SqlSyntax
{
    private readonly string _sql;

    public SqlSyntax(string sqlText)
    {
        _sql = sqlText;
    }

    internal Paragraph BuildStyledSqlParagraph(SqlTextStyles styles)
    {
        var tokens = SqlTokenizer.ParseText(_sql);
        var sortedTokens = tokens.OrderBy(s => s.Span.Start).ToList();
        var index = 0;

        var paragraph = new Paragraph();

        foreach (var token in sortedTokens)
        {
            var style = GetStyle(token.Kind, styles);

            if (index < token.Span.Start)
            {
                paragraph.Append(
                    _sql.Substring(index, token.Span.Start - index),
                    style
                );
            }

            var tokenText = _sql.Substring(token.Span.Start, token.Span.Length);
            Debug.Write(tokenText );
            paragraph.Append(tokenText, style);
            index = token.Span.End;
        }

        if (index < _sql.Length)
        {
            paragraph.Append(_sql.Substring(index), Color.DarkSlateGray1);
        }

        return paragraph;
    }

    private static Style GetStyle(SqlSyntaxKind kind, SqlTextStyles styles) => kind switch
    {
        SqlSyntaxKind.StringValue => styles.StringValueStyle,
        SqlSyntaxKind.Number => styles.NumberStyle,
        SqlSyntaxKind.Keyword => styles.KeywordStyle,
        SqlSyntaxKind.Identifier => styles.IdentifierStyle,
        SqlSyntaxKind.Operator => styles.OperatorStyle,
        SqlSyntaxKind.Comma => styles.CommaStyle,
        SqlSyntaxKind.Dot => styles.DotStyle,
        SqlSyntaxKind.OpenParenthesis => styles.OpenParenthesisStyle,
        SqlSyntaxKind.CloseParenthesis => styles.CloseParenthesisStyle,
        SqlSyntaxKind.SequenceTerminator => styles.SequenceTerminatorStyle,
        SqlSyntaxKind.NotDefined => styles.NotDefinedStyle,
        SqlSyntaxKind.Comment => styles.CommentStyle,
        _ => Color.White
    };
}

    