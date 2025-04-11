namespace Spectre.Console.Xml;

internal sealed class XmlTokenizer
{
    internal static List<XmlToken> ParseText(string xml)
    {
        var tokens = new List<XmlToken>();
        var span = xml.AsSpan();
        var i = 0;
        var length = span.Length;

        void AddToken(int start, int len, XmlSyntaxKind kind)
        {
            tokens.Add(new XmlToken(new TextSpan(start, len), kind));
        }

        bool IsWhitespace(char c) => c == ' ' || c == '\t' || c == '\n' || c == '\r';

        while (i < length)
        {
            var start = i;
            if (IsWhitespace(span[i]))
            {
                while (i < length && IsWhitespace(span[i]))
                {
                    i++;
                }

                AddToken(start, i - start, XmlSyntaxKind.Whitespace);
            }
            else if (span[i] == '<')
            {
                i++;

                if (i < length && span[i] == '!')
                {
                    if (i + 2 < length && span[i + 1] == '-' && span[i + 2] == '-')
                    {
                        // Comment
                        var commentStart = start;
                        i += 3;
                        while (i + 2 < length && !(span[i] == '-' && span[i + 1] == '-' && span[i + 2] == '>'))
                        {
                            i++;
                        }

                        i += 3;
                        AddToken(commentStart, i - commentStart, XmlSyntaxKind.Comment);
                    }
                    else if (i + 8 <= span.Length && span.Slice(i, 8).SequenceEqual("![CDATA[".AsSpan()))
                    {
                        // CDATA
                        var cdataStart = start;
                        i += 8;
                        while (i + 2 < length && !(span[i] == ']' && span[i + 1] == ']' && span[i + 2] == '>'))
                        {
                            i++;
                        }

                        i += 3;
                        AddToken(cdataStart, i - cdataStart, XmlSyntaxKind.CData);
                    }
                    else if (i + 8 <= span.Length && span.Slice(i, 8).SequenceEqual("!DOCTYPE".AsSpan()))
                    {
                        // DOCTYPE
                        var doctypeStart = start;
                        while (i < length && span[i] != '>')
                        {
                            i++;
                        }

                        i++;
                        AddToken(doctypeStart, i - doctypeStart, XmlSyntaxKind.DocumentTypeDeclaration);
                    }
                }
                else if (i < length && span[i] == '?')
                {
                    // ProcessingInstruction or Declaration
                    var piStart = start;
                    i++;
                    while (i + 1 < length && !(span[i] == '?' && span[i + 1] == '>'))
                    {
                        i++;
                    }

                    i += 2;
                    AddToken(piStart, i - piStart, XmlSyntaxKind.ProcessingInstruction); // Can distinguish if needed
                }
                else if (i < length && span[i] == '/')
                {
                    // End element
                    AddToken(start, 1, XmlSyntaxKind.OpeningAngleBracket);
                    i++;
                    var nameStart = i;
                    while (i < length && (char.IsLetterOrDigit(span[i]) || span[i] == ':' || span[i] == '-' || span[i] == '_'))
                    {
                        i++;
                    }

                    AddToken(nameStart, i - nameStart, XmlSyntaxKind.EndElement);
                    if (i < length && span[i] == '>')
                    {
                        AddToken(i++, 1, XmlSyntaxKind.ClosingAngleBracket);
                    }
                }
                else
                {
                    // Start tag or self-closing tag
                    AddToken(start, 1, XmlSyntaxKind.OpeningAngleBracket);
                    var nameStart = i;
                    while (i < length && (char.IsLetterOrDigit(span[i]) || span[i] == ':' || span[i] == '-' || span[i] == '_'))
                    {
                        i++;
                    }

                    AddToken(nameStart, i - nameStart, XmlSyntaxKind.ElementName);

                    // Attributes
                    while (i < length && span[i] != '>' && span[i] != '/')
                    {
                        if (IsWhitespace(span[i]))
                        {
                            var wsStart = i;
                            while (i < length && IsWhitespace(span[i]))
                            {
                                i++;
                            }

                            AddToken(wsStart, i - wsStart, XmlSyntaxKind.Whitespace);
                        }
                        else
                        {
                            // Attribute name
                            var attrNameStart = i;
                            while (i < length && span[i] != '=' && !IsWhitespace(span[i]))
                            {
                                i++;
                            }

                            AddToken(attrNameStart, i - attrNameStart, XmlSyntaxKind.AttributeName);

                            while (i < length && IsWhitespace(span[i]))
                            {
                                i++;
                            }

                            if (i < length && span[i] == '=')
                            {
                                AddToken(i, 1, XmlSyntaxKind.AttributeEquals);
                                i++;
                            }

                            while (i < length && IsWhitespace(span[i]))
                            {
                                i++;
                            }

                            if (i < length && (span[i] == '"' || span[i] == '\''))
                            {
                                var quote = span[i];
                                AddToken(i, 1, XmlSyntaxKind.AttributeQuote);
                                i++;
                                var valStart = i;
                                while (i < length && span[i] != quote)
                                {
                                    i++;
                                }

                                AddToken(valStart, i - valStart, XmlSyntaxKind.AttributeValue);
                                if (i < length && span[i] == quote)
                                {
                                    AddToken(i, 1, XmlSyntaxKind.AttributeQuote);
                                    i++;
                                }
                            }
                        }
                    }

                    if (i < length && span[i] == '/')
                    {
                        AddToken(i, 1, XmlSyntaxKind.SelfClosingSlash);
                        i++;
                    }

                    if (i < length && span[i] == '>')
                    {
                        AddToken(i, 1, XmlSyntaxKind.ClosingAngleBracket);
                        i++;
                    }
                }
            }
            else
            {
                // Text
                var textStart = i;
                while (i < length && span[i] != '<')
                {
                    i++;
                }

                AddToken(textStart, i - textStart, XmlSyntaxKind.Text);
            }
        }

        return tokens;
    }
}