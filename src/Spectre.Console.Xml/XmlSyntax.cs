using System.Diagnostics;

namespace Spectre.Console.Xml;

internal sealed class XmlSyntax
{
    private readonly string _xmlText;
    private readonly XmlBuilderContext _context;

    public XmlSyntax(string xmlText, XmlBuilderContext context)
    {
        _xmlText = xmlText;
        _context = context;
    }

    internal void Accept(XmlStyleBuilder xmlBuilder)
    {
        var tokens = XmlTokenizer.ParseText(_xmlText);
        xmlBuilder.Visit(tokens, _context);

        var sortedSegments = xmlBuilder.StyledSegments.OrderBy(s => s.Span.Start).ToList();
        var index = 0;

        foreach (var (span, style) in sortedSegments)
        {
            if (index < span.Start)
            {
                _context.Paragraph.Append(
                    _xmlText.Substring(index, span.Start - index),
                    style
                );
            }

            var tokenText = _xmlText.Substring(span.Start, span.Length);
            Debug.Write(tokenText );
            _context.Paragraph.Append(tokenText, style);
            index = span.End;
        }

        if (index < _xmlText.Length)
        {
            _context.Paragraph.Append(_xmlText.Substring(index), Color.DarkSlateGray1);
        }
    }
}