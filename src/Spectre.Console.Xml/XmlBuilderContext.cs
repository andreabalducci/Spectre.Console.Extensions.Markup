namespace Spectre.Console.Xml;

internal sealed class XmlBuilderContext
{
    internal XmlTextStyles Styling { get; }

    internal Paragraph Paragraph { get; }

    internal XmlBuilderContext(XmlTextStyles cSharpTextStyles)
    {
        Paragraph = new Paragraph();
        Styling = cSharpTextStyles;
    }
}
