using System.Diagnostics;

namespace Spectre.Console.Javascript;

internal sealed class JavascriptSyntax
{
    private readonly string _javascript;

    public JavascriptSyntax(string javascriptText)
    {
        _javascript = javascriptText;
    }

    internal Paragraph BuildStyledJavascriptParagraph(JavascriptTextStyles styles)
    {
        var tokens = JavascriptTokenizer.ParseText(_javascript);
        var sortedTokens = tokens.OrderBy(s => s.Span.Start).ToList();
        var index = 0;

        var paragraph = new Paragraph();

        foreach (var token in sortedTokens)
        {
            var style = GetStyle(token.Kind, styles);

            if (index < token.Span.Start)
            {
                paragraph.Append(
                    _javascript.Substring(index, token.Span.Start - index),
                    style
                );
            }

            var tokenText = _javascript.Substring(token.Span.Start, token.Span.Length);
            Debug.Write(tokenText );
            paragraph.Append(tokenText, style);
            index = token.Span.End;
        }

        if (index < _javascript.Length)
        {
            paragraph.Append(_javascript.Substring(index), Color.DarkSlateGray1);
        }

        return paragraph;
    }

    private static Style GetStyle(JavascriptSyntaxKind kind, JavascriptTextStyles styles) => kind switch
    {
        JavascriptSyntaxKind.NotDefined => styles.NotDefinedStyle,
        JavascriptSyntaxKind.And => styles.AndStyle,
        JavascriptSyntaxKind.Application => styles.ApplicationStyle,
        JavascriptSyntaxKind.Between => styles.BetweenStyle,
        JavascriptSyntaxKind.CloseParenthesis => styles.CloseParenthesisStyle,
        JavascriptSyntaxKind.Comma => styles.CommaStyle,
        JavascriptSyntaxKind.DateTimeValue => styles.DateTimeValueStyle,
        JavascriptSyntaxKind.Equals => styles.EqualsStyle,
        JavascriptSyntaxKind.ExceptionType => styles.ExceptionTypeStyle,
        JavascriptSyntaxKind.Fingerprint => styles.FingerprintStyle,
        JavascriptSyntaxKind.In => styles.InStyle,
        JavascriptSyntaxKind.Invalid => styles.InvalidStyle,
        JavascriptSyntaxKind.Like => styles.LikeStyle,
        JavascriptSyntaxKind.Limit => styles.LimitStyle,
        JavascriptSyntaxKind.Match => styles.MatchStyle,
        JavascriptSyntaxKind.Message => styles.MessageStyle,
        JavascriptSyntaxKind.NotEquals => styles.NotEqualsStyle,
        JavascriptSyntaxKind.NotIn => styles.NotInStyle,
        JavascriptSyntaxKind.NotLike => styles.NotLikeStyle,
        JavascriptSyntaxKind.Number => styles.NumberStyle,
        JavascriptSyntaxKind.Or => styles.OrStyle,
        JavascriptSyntaxKind.OpenParenthesis => styles.OpenParenthesisStyle,
        JavascriptSyntaxKind.StackFrame => styles.StackFrameStyle,
        JavascriptSyntaxKind.StringValue => styles.StringValueStyle,
        JavascriptSyntaxKind.SequenceTerminator => styles.SequenceTerminatorStyle,
        JavascriptSyntaxKind.Identifier => styles.IdentifierStyle,
        JavascriptSyntaxKind.Keyword => styles.KeywordStyle,
        JavascriptSyntaxKind.Comment => styles.CommentStyle,
        JavascriptSyntaxKind.Operator => styles.OperatorStyle,
        JavascriptSyntaxKind.Dot => styles.DotStyle,
        _ => Color.White
    };
}
