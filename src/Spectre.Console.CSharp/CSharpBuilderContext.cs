namespace Spectre.Console.CSharp;

internal class CSharpBuilderContext
{
    internal CSharpTextStyles Styling { get; }

    internal Paragraph Paragraph { get; }

    internal CSharpBuilderContext(CSharpTextStyles cSharpTextStyles)
    {
        Paragraph = new Paragraph();
        Styling = cSharpTextStyles;
    }
}
