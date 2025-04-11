namespace Spectre.Console.Xml;
internal class XmlStyleBuilder
{
    public static XmlStyleBuilder Shared { get; } = new XmlStyleBuilder();

    internal List<(TextSpan Span, Style Style)> StyledSegments { get; } = [];

    internal void Visit(List<XmlToken> tree, XmlBuilderContext context)
    {
        foreach (var token in tree)
        {
            var style = token.Kind switch
            {
                XmlSyntaxKind.ElementName => context.Styling.ElementNameStyle,
                XmlSyntaxKind.Text => context.Styling.TextStyle,
                XmlSyntaxKind.Comment => context.Styling.CommentStyle,
                XmlSyntaxKind.ProcessingInstruction => context.Styling.ProcessingInstructionStyle,
                XmlSyntaxKind.DocumentTypeDeclaration => context.Styling.DocumentTypeDeclarationStyle,
                XmlSyntaxKind.CData => context.Styling.CDataStyle,
                XmlSyntaxKind.Whitespace => context.Styling.WhitespaceStyle,
                XmlSyntaxKind.EndElement => context.Styling.EndElementStyle,
                XmlSyntaxKind.OpeningAngleBracket => context.Styling.OpeningAngleBracketStyle,
                XmlSyntaxKind.ClosingAngleBracket => context.Styling.ClosingAngleBracketStyle,
                XmlSyntaxKind.AttributeName => context.Styling.AttributeNameStyle,
                XmlSyntaxKind.AttributeEquals => context.Styling.AttributeEqualsStyle,
                XmlSyntaxKind.AttributeValue => context.Styling.AttributeValueStyle,
                XmlSyntaxKind.AttributeQuote => context.Styling.AttributeQuoteStyle,
                XmlSyntaxKind.SelfClosingSlash => context.Styling.SelfClosingSlashStyle,
                _ => Color.White
            };

            StyledSegments.Add((token.Span, style));
        }
    }
}
