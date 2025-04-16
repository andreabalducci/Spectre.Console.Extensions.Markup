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
    public Style? CodeBlockBorderStyle { get; set; }
    public Padding? CodeBlockPadding { get; set; }
    public BoxBorder? CodeBlockBorder { get; set; }

    public Color? HeadingLevel1Color { get; set; }
    public Style? HeadingLevel2To4Style { get; set; }
    public Style? HeadingLevel5AndAboveStyle { get; set; }

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
            HeadingLevel5AndAboveStyle = HeadingLevel5AndAboveStyle ?? Color.Yellow
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
