using Markdig;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup;
public sealed class MarkdownRenderable(string markdown) : Renderable
{
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var ast = Markdown.Parse(markdown, MakePipeline());

        var blockRenderer = new BlockRenderer();

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
