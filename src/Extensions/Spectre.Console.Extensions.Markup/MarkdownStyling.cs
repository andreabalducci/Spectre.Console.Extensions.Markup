namespace Spectre.Console.Extensions.Markup;
internal class MarkdownStyling
{
    public Style CodeBlockBorderStyle { get; set; } = default!;
    public Padding CodeBlockPadding { get; set; } = default!;
    public BoxBorder CodeBlockBorder { get; set; } = default!;

    public Color HeadingLevel1Color { get; set; } = default!;
    public Style HeadingLevel2To4Style { get; set; } = default!;
    public Style HeadingLevel5AndAboveStyle { get; set; } = default!;

    public char ListBlockMarker { get; set; }
    public Style ListBlockMarkerStyle { get; set; } = default!;
}
