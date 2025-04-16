using Markdig.Syntax;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class QuoteBlockRenderer : IRenderer<QuoteBlock>
{
    private readonly InlineRenderer _inlineRendering = new();
    private readonly BlockRenderer _blockRenderer;

    public QuoteBlockRenderer(Dictionary<string, Func<string, JustInTimeRenderable>> codeblockRenderables, MarkdownStyling styling)
    {
        _blockRenderer = new BlockRenderer(codeblockRenderables, styling);
    }

    public IRenderable Render(QuoteBlock quoteBlock)
    {
        var renderables = new CompositeRenderable(quoteBlock.Select(x =>
        {
            if (x is ParagraphBlock { Inline: not null } paragraphBlock)
            {
                return _inlineRendering.RenderInline(paragraphBlock.Inline, Style.Plain, Justify.Left);
            }
            else
            {
                return _blockRenderer.Render(x);
            }
        }));

        var panel = new Panel(renderables)
        {
            Border = new LeftBoxBorder(),
            BorderStyle = new Style(Color.Green),
            Padding = new Padding(1, 1)
        };

        return panel;
    }
}
