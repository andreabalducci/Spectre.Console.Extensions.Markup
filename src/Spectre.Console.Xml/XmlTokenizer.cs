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

/*
 
* ------------------------------------------------------------------------------- *
   This was (mainly) build with the following prompt (chatgpt):
* ------------------------------------------------------------------------------- *
  
 Can you write a XML parser in C#. You should create a Parse method with

- input: xml string
- output: List<(start, length, XmlSyntaxType>

where start is the position (index) in the xml string.

XmlSyntaxType is defined as follows:

```csharp
internal enum XmlSyntaxKind
{
    None,           // Represents no specific kind
    ElementName,    // Represents an XML element name. eg <element>
    Text,           // Represents text content within an XML element
    Comment,        // Represents an XML comment. Everything including <!-- ... -->
    ProcessingInstruction, // Represents an XML processing instruction. e.g. <?xml version="1.0"?>
    DocumentTypeDeclaration // A document type declaration, or DOCTYPE, is an instruction that associates a particular XML or SGML document (for example, a web page) with a document type definition (DTD)
    CData,          // Represents a CDATA section
    Whitespace,     // Represents whitespace
    EndElement,     // Represents the end of an XML element (e.g. <element/> )
    Declaration,    // Represents an XML declaration (e.g., <?xml version="1.0"?>)
    OpeningAngleBracket, // <
    ClosingAngleBracket, // >
    AttributeName,   // Name of the attribute
    AttributeEquals, // =
    AttributeValue,  // Represents the value of an XML attribute
    AttributeQuote,  // " or '
    SelfClosingSlash,  // / => Represents a self-closing tag (e.g., <element/>)
}
```

Example: If you encounter <element/>, you will list OpeningAngleBracket (<), EndElement (element), SelfClosingSlash (/) and ClosingAngleBracket (>) 

Do not use XmlReader or any other library or package. Just go through the string.

It should be able to parse the following xml, although it may not be valid:

```xml
 <!DOCTYPE glossary PUBLIC "-//OASIS//DTD DocBook V3.1//EN">
 <glossary><title>example glossary</title>
  <GlossDiv><title>S</title>
   <GlossList>
    <GlossEntry ID="SGML" SortAs="SGML">
     <GlossTerm>Standard Generalized Markup Language</GlossTerm>
     <Acronym>SGML</Acronym>
     <Abbrev>ISO 8879:1986</Abbrev>
     <GlossDef>
      <para>A meta-markup language, used to create markup
languages such as DocBook.</para>
      <GlossSeeAlso OtherTerm="GML">
      <GlossSeeAlso OtherTerm="XML">
     </GlossDef>
     <GlossSee OtherTerm="markup">
    </GlossEntry>
   </GlossList>
  </GlossDiv>
 </glossary>
```

The result of the method will be used to write syntax highlighting. so xml validity is less important. 
*/