
using Spectre.Console.Rendering;

namespace Spectre.Console.Xml;

/// <summary>
/// A renderable piece of XML text.
/// </summary>
public sealed class XmlText : JustInTimeRenderable
{
    private readonly string _xml;

    /// <summary>
    /// Gets or sets the style for XML element names.
    /// </summary>
    public Style? ElementNameStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for text content within XML elements.
    /// </summary>
    public Style? TextStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for XML comments.
    /// </summary>
    public Style? CommentStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for XML processing instructions.
    /// </summary>
    public Style? ProcessingInstructionStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for document type declarations.
    /// </summary>
    public Style? DocumentTypeDeclarationStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for CDATA sections.
    /// </summary>
    public Style? CDataStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for whitespace in the XML document.
    /// </summary>
    public Style? WhitespaceStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for end elements.
    /// </summary>
    public Style? EndElementStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for opening angle brackets. <
    /// </summary>
    public Style? OpeningAngleBracketStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for closing angle brackets. >
    /// </summary>
    public Style? ClosingAngleBracketStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for attribute names.
    /// </summary>
    public Style? AttributeNameStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for the equals sign in attributes.
    /// </summary>
    public Style? AttributeEqualsStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for attribute values.
    /// </summary>
    public Style? AttributeValueStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for quotes in attributes.
    /// </summary>
    public Style? AttributeQuoteStyle { get; set; } = default!;

    /// <summary>
    /// Gets or sets the style for self-closing slashes.
    /// </summary>
    public Style? SelfClosingSlashStyle { get; set; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlText"/> class.
    /// </summary>
    /// <param name="xml">The C# to render.</param>
    public XmlText(string? xml)
    {
        _xml = xml?.Trim() ?? throw new ArgumentNullException(nameof(xml));
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        var context = new XmlBuilderContext(
            new XmlTextStyles
            {
                ElementNameStyle = ElementNameStyle ?? Color.DeepSkyBlue3_1,
                TextStyle = TextStyle ?? Color.DarkSlateGray1,
                CommentStyle = CommentStyle ?? new Style(foreground: Color.FromHex("006400")),
                ProcessingInstructionStyle = ProcessingInstructionStyle ?? Color.Turquoise2,
                DocumentTypeDeclarationStyle = DocumentTypeDeclarationStyle ?? Color.Turquoise2,
                CDataStyle = CDataStyle ?? Color.Magenta1,
                WhitespaceStyle = WhitespaceStyle ?? Color.Yellow,
                EndElementStyle = EndElementStyle ?? Color.DeepSkyBlue3_1,
                OpeningAngleBracketStyle = OpeningAngleBracketStyle ?? Color.DeepSkyBlue4_2,
                ClosingAngleBracketStyle = ClosingAngleBracketStyle ?? Color.DeepSkyBlue4_2,
                AttributeNameStyle = AttributeNameStyle ?? Color.Turquoise2,
                AttributeEqualsStyle = AttributeEqualsStyle ?? Color.DeepSkyBlue4_2,
                AttributeValueStyle = AttributeValueStyle ?? Color.White,
                AttributeQuoteStyle = AttributeQuoteStyle ?? Color.DeepSkyBlue3_1,
                SelfClosingSlashStyle = SelfClosingSlashStyle ?? Color.DeepSkyBlue4_2
            }
        );

        var syntax = new XmlSyntax(_xml, context);
        
        syntax.Accept(XmlStyleBuilder.Shared);
        return context.Paragraph;
    }
}
