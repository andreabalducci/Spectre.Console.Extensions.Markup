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
        var blockRenderer = new BlockRenderer(CodeblockRenderables);

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
