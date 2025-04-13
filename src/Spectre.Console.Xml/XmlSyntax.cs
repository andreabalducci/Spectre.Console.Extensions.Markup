using System.Diagnostics;

namespace Spectre.Console.Xml;

internal sealed class XmlSyntax
{
    private readonly string _xml;

    public XmlSyntax(string xmlText)
    {
        _xml = xmlText;
    }

    internal Paragraph BuildStyledXmlParagraph(XmlTextStyles styles)
    {
        var tokens = XmlTokenizer.ParseText(_xml);
        var sortedTokens = tokens.OrderBy(s => s.Span.Start).ToList();
        var index = 0;

        var paragraph = new Paragraph();

        foreach (var token in sortedTokens)
        {
            var style = GetStyle(token.Kind, styles);

            if (index < token.Span.Start)
            {
                paragraph.Append(
                    _xml.Substring(index, token.Span.Start - index),
                    style
                );
            }

            var tokenText = _xml.Substring(token.Span.Start, token.Span.Length);
            Debug.Write(tokenText );
            paragraph.Append(tokenText, style);
            index = token.Span.End;
        }

        if (index < _xml.Length)
        {
            paragraph.Append(_xml.Substring(index), Color.DarkSlateGray1);
        }

        return paragraph;
    }

    private static Style GetStyle(XmlSyntaxKind kind, XmlTextStyles styles) => kind switch
    {
        XmlSyntaxKind.ElementName => styles.ElementNameStyle,
        XmlSyntaxKind.Text => styles.TextStyle,
        XmlSyntaxKind.Comment => styles.CommentStyle,
        XmlSyntaxKind.ProcessingInstruction => styles.ProcessingInstructionStyle,
        XmlSyntaxKind.DocumentTypeDeclaration => styles.DocumentTypeDeclarationStyle,
        XmlSyntaxKind.CData => styles.CDataStyle,
        XmlSyntaxKind.Whitespace => styles.WhitespaceStyle,
        XmlSyntaxKind.EndElement => styles.EndElementStyle,
        XmlSyntaxKind.OpeningAngleBracket => styles.OpeningAngleBracketStyle,
        XmlSyntaxKind.ClosingAngleBracket => styles.ClosingAngleBracketStyle,
        XmlSyntaxKind.AttributeName => styles.AttributeNameStyle,
        XmlSyntaxKind.AttributeEquals => styles.AttributeEqualsStyle,
        XmlSyntaxKind.AttributeValue => styles.AttributeValueStyle,
        XmlSyntaxKind.AttributeQuote => styles.AttributeQuoteStyle,
        XmlSyntaxKind.SelfClosingSlash => styles.SelfClosingSlashStyle,
        _ => Color.White
    };
}