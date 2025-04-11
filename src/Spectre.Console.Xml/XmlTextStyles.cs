using System.Xml.Linq;

namespace Spectre.Console.Xml;

internal sealed class XmlTextStyles
{
    public Style ElementNameStyle {get;set;} = default!;
    public Style TextStyle {get;set;} = default!;
    public Style CommentStyle {get;set;} = default!;
    public Style ProcessingInstructionStyle {get;set;} = default!;
    public Style DocumentTypeDeclarationStyle { get; set; } = default!;
    public Style CDataStyle {get;set;} = default!;
    public Style WhitespaceStyle {get;set;} = default!;
    public Style EndElementStyle {get;set;} = default!;
    public Style OpeningAngleBracketStyle {get;set;} = default!;
    public Style ClosingAngleBracketStyle {get;set;} = default!;
    public Style AttributeNameStyle {get;set;} = default!;
    public Style AttributeEqualsStyle {get;set;} = default!;
    public Style AttributeValueStyle {get;set;} = default!;
    public Style AttributeQuoteStyle {get;set;} = default!;
    public Style SelfClosingSlashStyle { get; set; } = default!;
}
