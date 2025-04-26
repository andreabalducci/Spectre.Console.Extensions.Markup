using Markdig;
using Spectre.Console.CSharp;
using Spectre.Console.Extensions.Markup.Renderers;
using Spectre.Console.Javascript;
using Spectre.Console.Json;
using Spectre.Console.Rendering;
using Spectre.Console.Sql;
using Spectre.Console.Xml;

namespace Spectre.Console.Extensions.Markup;
public sealed class MarkdownRenderable(string markdown) : Renderable
{
    /// <summary>
    /// Gets or sets the style for the border of a code block.
    /// </summary>
    public Style? CodeBlockBorderStyle { get; set; }

    /// <summary>
    /// Gets or sets the padding for a code block.
    /// </summary>
    public Padding? CodeBlockPadding { get; set; }

    /// <summary>
    /// Gets or sets the border type for a code block.
    /// </summary>
    public BoxBorder? CodeBlockBorder { get; set; }

    /// <summary>
    /// Gets or sets the color for level 1 headings.
    /// </summary>
    public Color? HeadingLevel1Color { get; set; }

    /// <summary>
    /// Gets or sets the style for level 2 to 4 headings.
    /// </summary>
    public Style? HeadingLevel2To4Style { get; set; }

    /// <summary>
    /// Gets or sets the style for level 5 and above headings.
    /// </summary>
    public Style? HeadingLevel5AndAboveStyle { get; set; }

    /// <summary>
    /// Gets or sets the marker character for list blocks.
    /// </summary>
    public char? ListBlockMarker { get; set; }

    /// <summary>
    /// Gets or sets the style for the marker of list blocks.
    /// </summary>
    public Style? ListBlockMarkerStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the border of a quote block.
    /// </summary>
    public Style? QuoteBlockBorderStyle { get; set; }

    /// <summary>
    /// Gets or sets the padding for a quote block.
    /// </summary>
    public Padding? QuoteBlockPadding { get; set; }

    /// <summary>
    /// Gets or sets the border type for a quote block.
    /// </summary>
    public BoxBorder? QuoteBlockBorder { get; set; }

    /// <summary>
    /// Gets or sets the style for the border of a table.
    /// </summary>
    public Color? TableBorderStyle { get; set; }

    /// <summary>
    /// Gets or sets the border type for a table.
    /// </summary>
    public TableBorder? TableBorder { get; set; }

    public readonly Dictionary<string, Func<string, JustInTimeRenderable>> CodeblockRenderables =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "json", code => new JsonText(code) },
            { "javascript", code => new JavascriptText(code) },
            { "csharp", code => new CSharpText(code) },
            { "xml", code => new XmlText(code) },
            { "sql", code => new SqlText(code) }
        };

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var ast = Markdown.Parse(markdown, MakePipeline());
        var styling = new MarkdownStyling
        {
            CodeBlockBorderStyle = CodeBlockBorderStyle ?? Color.Blue,
            CodeBlockPadding = CodeBlockPadding ?? new Padding(1, 0, 0, 0),
            CodeBlockBorder = CodeBlockBorder ?? new LeftBoxBorder(),
            HeadingLevel1Color = HeadingLevel1Color ?? Color.White,
            HeadingLevel2To4Style = HeadingLevel2To4Style ?? Color.Yellow,
            HeadingLevel5AndAboveStyle = HeadingLevel5AndAboveStyle ?? Color.Yellow,
            ListBlockMarker = ListBlockMarker ?? '\u25cb',
            ListBlockMarkerStyle = ListBlockMarkerStyle ?? Color.Green,
            QuoteBlockBorderStyle = QuoteBlockBorderStyle ?? Color.Green,
            QuoteBlockPadding = QuoteBlockPadding ?? new Padding(1, 1),
            QuoteBlockBorder = QuoteBlockBorder ?? new LeftBoxBorder(),
            TableBorderStyle = TableBorderStyle ?? Color.White,
            TableBorder = TableBorder ?? TableBorder.Square

        };
        var blockRenderer = new BlockRenderer(CodeblockRenderables, styling);

        foreach (var block in ast)
        {
            var r = blockRenderer.Render(block).Render(options, maxWidth);
            foreach (var segment in r)
            {
                yield return segment;
            }

            yield return Segment.LineBreak;
        }
    }

    private static MarkdownPipeline MakePipeline() =>
        new MarkdownPipelineBuilder()
            .UseAutoIdentifiers()
            .UseEmphasisExtras()
            .UseGridTables()
            .UseMediaLinks()
            .UsePipeTables()
            .UseListExtras()
            .UseDiagrams()
            .UseGenericAttributes()
            .UseEmojiAndSmiley()
            .Build();
}
